#!/usr/bin/env python3
"""
Train Collaborative Filtering (implicit ALS) on Tracking.user_item_interactions
and write embeddings into:
  - Tracking.user_embeddings_cf
  - Tracking.item_embeddings_cf

Assumptions:
  - You already aggregated events_raw -> user_item_interactions.
  - DB has pgvector extension and tables exist (SignalsDbContext migrations applied).
  - DB columns are vector(128) matching the default --factors value.

Model:
  - Implicit feedback ALS (library: implicit)

Usage:
  export DATABASE_URL="postgresql://USER:PASSWORD@HOST:5432/DBNAME"
  python3 scripts/train_cf_als_write_embeddings.py --model-ver als128_v1 --factors 128

Notes:
  - Requires: pip install implicit numpy scipy psycopg2-binary pgvector
  - On macOS, implicit may require build tools; if it's painful, we can switch
    to a pure-numpy baseline or item-item kNN first.
"""

from __future__ import annotations

import argparse
import os
from datetime import datetime, timezone
from typing import Dict, List, Tuple

import numpy as np
import psycopg2
from psycopg2.extras import execute_values
from scipy.sparse import coo_matrix


def _vec_to_pgvector_literal(vec: np.ndarray) -> str:
    # pgvector text input format: '[1,2,3]'
    return "[" + ",".join(f"{float(x):.8f}" for x in vec.tolist()) + "]"


def _fetch_interactions(conn, min_weight: float = 0.0) -> Tuple[List[str], List[str], np.ndarray]:
    """
    Returns:
      user_keys: list of distinct UserKey
      item_ids: list of distinct ItemId
      matrix: COO (user_idx, item_idx, weight)
    """
    with conn.cursor() as cur:
        cur.execute(
            """
            SELECT "UserKey", "ItemId", "Weight"
            FROM "Tracking"."user_item_interactions"
            WHERE "Weight" > %s
            """,
            (min_weight,),
        )
        rows = cur.fetchall()

    user_keys = sorted({r[0] for r in rows})
    item_ids = sorted({r[1] for r in rows})
    user_index: Dict[str, int] = {u: i for i, u in enumerate(user_keys)}
    item_index: Dict[str, int] = {it: i for i, it in enumerate(item_ids)}

    ui = np.fromiter((user_index[r[0]] for r in rows), dtype=np.int32, count=len(rows))
    ii = np.fromiter((item_index[r[1]] for r in rows), dtype=np.int32, count=len(rows))
    vv = np.fromiter((float(r[2]) for r in rows), dtype=np.float32, count=len(rows))
    mat = coo_matrix((vv, (ui, ii)), shape=(len(user_keys), len(item_ids)), dtype=np.float32)
    return user_keys, item_ids, mat


def main() -> int:
    parser = argparse.ArgumentParser(description="Train implicit ALS and write embeddings to Postgres")
    parser.add_argument("--db-url", default=None, help="Postgres connection string (or env DATABASE_URL).")
    parser.add_argument("--min-weight", type=float, default=0.0, help="Filter out interactions with Weight <= min-weight.")
    parser.add_argument("--factors", type=int, default=128, help="ALS latent factors (default 128).")
    parser.add_argument("--iterations", type=int, default=20, help="ALS iterations (default 20).")
    parser.add_argument("--regularization", type=float, default=0.01, help="ALS regularization (default 0.01).")
    parser.add_argument("--alpha", type=float, default=40.0, help="Confidence scaling alpha for implicit ALS (default 40).")
    parser.add_argument("--model-ver", required=True, help="Model version string saved to DB (e.g., als128_v1).")
    parser.add_argument("--batch-size", type=int, default=2000, help="Upsert batch size (default 2000).")
    args = parser.parse_args()

    db_url = args.db_url or os.environ.get("DATABASE_URL")
    if not db_url:
        raise SystemExit("Provide --db-url or set env DATABASE_URL")

    # Local import to keep script runnable even if implicit isn't installed yet
    try:
        from implicit.als import AlternatingLeastSquares
    except Exception as e:
        raise SystemExit(
            "Missing dependency: implicit. Install with: pip install implicit\n"
            f"Original error: {e}"
        )

    print("Connecting to DB...")
    conn = psycopg2.connect(db_url)
    conn.autocommit = False

    try:
        print("Fetching user-item interactions...")
        user_keys, item_ids, mat = _fetch_interactions(conn, min_weight=args.min_weight)
        print(f"Users: {len(user_keys)}, Items: {len(item_ids)}, Non-zeros: {mat.nnz}")

        # implicit expects item-user matrix (items x users)
        item_user = mat.tocsr().T.tocsr()

        print("Training implicit ALS...")
        model = AlternatingLeastSquares(
            factors=args.factors,
            iterations=args.iterations,
            regularization=args.regularization,
            alpha=args.alpha,
        )
        model.fit(item_user)

        trained_at = datetime.now(tz=timezone.utc)

        # IMPORTANT: We passed item_user (items × users) to fit(), so:
        # - model.user_factors corresponds to COLUMNS = our USERS
        # - model.item_factors corresponds to ROWS = our ITEMS
        # This seems backwards, but implicit interprets the matrix as (users × items)
        # so when we transpose, the naming gets swapped.
        #
        # Actually, let's check what we have:
        raw_user_factors = np.asarray(model.user_factors).astype(np.float32, copy=False)
        raw_item_factors = np.asarray(model.item_factors).astype(np.float32, copy=False)

        print(f"model.user_factors shape: {raw_user_factors.shape}")
        print(f"model.item_factors shape: {raw_item_factors.shape}")
        print(f"Expected: users={len(user_keys)}, items={len(item_ids)}")

        # We fed model.fit(item_user) where item_user has shape (n_items, n_users)
        # implicit expects (n_users, n_items), so it interprets:
        #   - rows (n_items) as "users" → model.user_factors has shape (n_items, factors)
        #   - cols (n_users) as "items" → model.item_factors has shape (n_users, factors)
        # So we need to SWAP them:
        user_factors = raw_item_factors  # actual user embeddings
        item_factors = raw_user_factors  # actual item embeddings

        print(f"After swap: user_factors={user_factors.shape}, item_factors={item_factors.shape}")

        if user_factors.shape[0] != len(user_keys):
            raise RuntimeError(f"user_factors mismatch: {user_factors.shape[0]} vs {len(user_keys)}")
        if item_factors.shape[0] != len(item_ids):
            raise RuntimeError(f"item_factors mismatch: {item_factors.shape[0]} vs {len(item_ids)}")

        print(f"Upserting user_embeddings_cf and item_embeddings_cf (dim={args.factors})...")
        with conn.cursor() as cur:
            user_rows = []
            for i, user_key in enumerate(user_keys):
                emb = user_factors[i].astype(np.float32, copy=False)
                user_rows.append((user_key, _vec_to_pgvector_literal(emb), args.model_ver, trained_at))
                if len(user_rows) >= args.batch_size:
                    execute_values(
                        cur,
                        """
                        INSERT INTO "Tracking"."user_embeddings_cf" ("UserKey","Emb","ModelVer","TrainedAt")
                        VALUES %s
                        ON CONFLICT ("UserKey") DO UPDATE
                        SET "Emb" = EXCLUDED."Emb",
                            "ModelVer" = EXCLUDED."ModelVer",
                            "TrainedAt" = EXCLUDED."TrainedAt"
                        """,
                        user_rows,
                        template="(%s,%s::vector,%s,%s)",
                        page_size=len(user_rows),
                    )
                    user_rows = []

            if user_rows:
                execute_values(
                    cur,
                    """
                    INSERT INTO "Tracking"."user_embeddings_cf" ("UserKey","Emb","ModelVer","TrainedAt")
                    VALUES %s
                    ON CONFLICT ("UserKey") DO UPDATE
                    SET "Emb" = EXCLUDED."Emb",
                        "ModelVer" = EXCLUDED."ModelVer",
                        "TrainedAt" = EXCLUDED."TrainedAt"
                    """,
                    user_rows,
                    template="(%s,%s::vector,%s,%s)",
                    page_size=len(user_rows),
                )

            item_rows = []
            for i, item_id in enumerate(item_ids):
                emb = item_factors[i].astype(np.float32, copy=False)
                item_rows.append((item_id, _vec_to_pgvector_literal(emb), args.model_ver, trained_at))
                if len(item_rows) >= args.batch_size:
                    execute_values(
                        cur,
                        """
                        INSERT INTO "Tracking"."item_embeddings_cf" ("ItemId","Emb","ModelVer","TrainedAt")
                        VALUES %s
                        ON CONFLICT ("ItemId") DO UPDATE
                        SET "Emb" = EXCLUDED."Emb",
                            "ModelVer" = EXCLUDED."ModelVer",
                            "TrainedAt" = EXCLUDED."TrainedAt"
                        """,
                        item_rows,
                        template="(%s,%s::vector,%s,%s)",
                        page_size=len(item_rows),
                    )
                    item_rows = []

            if item_rows:
                execute_values(
                    cur,
                    """
                    INSERT INTO "Tracking"."item_embeddings_cf" ("ItemId","Emb","ModelVer","TrainedAt")
                    VALUES %s
                    ON CONFLICT ("ItemId") DO UPDATE
                    SET "Emb" = EXCLUDED."Emb",
                        "ModelVer" = EXCLUDED."ModelVer",
                        "TrainedAt" = EXCLUDED."TrainedAt"
                    """,
                    item_rows,
                    template="(%s,%s::vector,%s,%s)",
                    page_size=len(item_rows),
                )

        conn.commit()
        print("Done.")
        return 0
    except Exception:
        conn.rollback()
        raise
    finally:
        conn.close()


if __name__ == "__main__":
    raise SystemExit(main())


