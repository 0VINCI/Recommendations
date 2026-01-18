#!/usr/bin/env python3
"""
Import RetailRocket events.csv into your Tracking.events_raw table (PostgreSQL),
mapping RetailRocket `itemid` -> your Product GUIDs.

Why:
  - You want a shared product-id space across algorithms (CF vs content-based vs YOLO).
  - RetailRocket provides realistic implicit-feedback interactions (view/addtocart/transaction).
  - This script maps top interacted RR items to your existing products and bulk-inserts events_raw.

Input files:
  1) RetailRocket events.csv:
     columns: timestamp,visitorid,event,itemid,transactionid
  2) Your products CSV export:
     columns: Id,ExternalId  (Id must be GUID)
     example:
       Id,ExternalId
       0003d25a-...-4f81fcf2765e,20406

Output:
  - Inserts rows into: "Tracking"."events_raw"
  - Optionally writes the mapping to a CSV for reproducibility.

Event mapping:
  RR "view"        -> Type "product_viewed"   Source "frontend"
  RR "addtocart"   -> Type "cart_item_added"  Source "backend"
  RR "transaction" -> Type "order_placed"     Source "backend"

Payload format (kept minimal, consistent with computed columns in DB):
  - Always includes "item_id" (GUID string) for item-related events
  - For transaction: also includes "order_id" (transactionid as string)

Connection:
  Provide --db-url (preferred) or --db-host/--db-port/--db-name/--db-user/--db-password.
  You can also set env var DATABASE_URL and omit --db-url.

Examples:
  # Dry run (no DB) just create mapping CSV
  python3 scripts/retailrocket_import_to_events_raw.py \\
    --events /Users/pawelw/Downloads/archive/events.csv \\
    --products-csv /Users/pawelw/Downloads/archive/postgres_Dictionary_Products.csv \\
    --out-mapping /Users/pawelw/Desktop/magisterka/programowanie/backend/Recommendations/rr_reports/mapping_rr_to_guid.csv \\
    --dry-run

  # Import first 200k events (test)
  python3 scripts/retailrocket_import_to_events_raw.py \\
    --events /Users/pawelw/Downloads/archive/events.csv \\
    --products-csv /Users/pawelw/Downloads/archive/postgres_Dictionary_Products.csv \\
    --db-url "$DATABASE_URL" \\
    --limit 200000
"""

from __future__ import annotations

import argparse
import csv
import os
import uuid
from collections import Counter
from dataclasses import dataclass
from datetime import datetime, timezone
from pathlib import Path
from typing import Dict, Iterable, List, Optional, Tuple

import psycopg2
from psycopg2.extras import Json, execute_values


@dataclass(frozen=True)
class ProductRow:
    product_id: str  # GUID string
    external_id: Optional[int]


def _is_guid(s: str) -> bool:
    try:
        uuid.UUID((s or "").strip())
        return True
    except Exception:
        return False


def _parse_int(s: str) -> Optional[int]:
    s = (s or "").strip()
    if not s:
        return None
    try:
        return int(s)
    except ValueError:
        return None


def _read_products_csv(path: Path, encoding: str) -> List[ProductRow]:
    with path.open("r", encoding=encoding, newline="") as f:
        reader = csv.DictReader(f)
        if reader.fieldnames is None:
            raise ValueError(f"Empty CSV: {path}")

        # detect columns case-insensitively
        cols = {c.lower(): c for c in reader.fieldnames}
        id_col = cols.get("id") or cols.get("productid") or cols.get("product_id")
        ext_col = cols.get("externalid") or cols.get("external_id")
        if not id_col:
            raise ValueError(f"Products CSV must have Id column. Found: {reader.fieldnames}")

        products: List[ProductRow] = []
        for row in reader:
            pid = (row.get(id_col) or "").strip()
            if not pid or not _is_guid(pid):
                continue
            ext = _parse_int(row.get(ext_col, "")) if ext_col else None
            products.append(ProductRow(product_id=pid, external_id=ext))

    if not products:
        raise ValueError(f"No valid GUID product ids found in {path}")
    # deterministic sorting: by ExternalId (if present), then by GUID
    products_sorted = sorted(
        products,
        key=lambda p: (
            p.external_id is None,
            p.external_id if p.external_id is not None else 10**18,
            p.product_id,
        ),
    )
    return products_sorted


def _iter_events(events_path: Path, encoding: str) -> Iterable[Dict[str, str]]:
    with events_path.open("r", encoding=encoding, newline="") as f:
        reader = csv.DictReader(f)
        if reader.fieldnames is None:
            return
        for row in reader:
            yield row


def _count_rr_item_interactions(events_path: Path, encoding: str) -> Counter:
    counts: Counter = Counter()
    for row in _iter_events(events_path, encoding):
        raw = (row.get("itemid") or "").strip()
        if not raw:
            continue
        try:
            rr_itemid = int(raw)
        except ValueError:
            continue
        counts[rr_itemid] += 1
    return counts


def _build_mapping_top_rr_to_products(
    rr_item_counts: Counter,
    products_sorted: List[ProductRow],
) -> Dict[int, str]:
    rr_items_sorted = [itemid for itemid, _ in rr_item_counts.most_common()]
    n = min(len(rr_items_sorted), len(products_sorted))
    return {rr_items_sorted[i]: products_sorted[i].product_id for i in range(n)}


def _write_mapping_csv(mapping: Dict[int, str], out_path: Path) -> None:
    out_path.parent.mkdir(parents=True, exist_ok=True)
    with out_path.open("w", encoding="utf-8", newline="") as f:
        w = csv.writer(f)
        w.writerow(["rr_itemid", "product_id"])
        for rr_itemid, pid in sorted(mapping.items(), key=lambda kv: kv[0]):
            w.writerow([rr_itemid, pid])


def _to_dt_from_ms(ts_ms: int) -> datetime:
    return datetime.fromtimestamp(ts_ms / 1000.0, tz=timezone.utc)


def _map_event(rr_event: str) -> Optional[Tuple[str, str]]:
    """
    Returns (Type, SourceLabel) where SourceLabel matches PgName of enum rec.event_source.
    """
    rr_event = (rr_event or "").strip().lower()
    if rr_event == "view":
        return "product_viewed", "frontend"
    if rr_event == "addtocart":
        return "cart_item_added", "backend"
    if rr_event == "transaction":
        return "order_placed", "backend"
    return None


def _connect(db_url: Optional[str], host: str, port: int, dbname: str, user: str, password: str):
    if db_url:
        return psycopg2.connect(db_url)
    return psycopg2.connect(host=host, port=port, database=dbname, user=user, password=password)


def main() -> int:
    parser = argparse.ArgumentParser(description="Import RetailRocket events to Tracking.events_raw")
    parser.add_argument("--events", required=True, help="Path to RetailRocket events.csv")
    parser.add_argument("--products-csv", required=True, help="Path to Products export CSV (Id,ExternalId)")
    parser.add_argument("--encoding", default="utf-8", help="CSV encoding (try latin-1 if needed)")
    parser.add_argument("--out-mapping", default=None, help="Optional output mapping CSV path")
    parser.add_argument("--limit", type=int, default=0, help="Limit number of event rows processed (0 = no limit)")
    parser.add_argument("--batch-size", type=int, default=5000, help="DB insert batch size (default 5000)")
    parser.add_argument("--dry-run", action="store_true", help="Do not connect/insert to DB; only build mapping")

    # DB connection options
    parser.add_argument("--db-url", default=None, help="Postgres connection string. If omitted, uses env DATABASE_URL.")
    parser.add_argument("--db-host", default="localhost")
    parser.add_argument("--db-port", type=int, default=5432)
    parser.add_argument("--db-name", default="postgres")
    parser.add_argument("--db-user", default="postgres")
    parser.add_argument("--db-password", default="")

    args = parser.parse_args()

    events_path = Path(args.events).expanduser().resolve()
    products_path = Path(args.products_csv).expanduser().resolve()
    if not events_path.exists():
        raise FileNotFoundError(f"events.csv not found: {events_path}")
    if not products_path.exists():
        raise FileNotFoundError(f"products csv not found: {products_path}")

    print("Reading products CSV...")
    products = _read_products_csv(products_path, encoding=args.encoding)
    print(f"Products loaded: {len(products)}")

    print("Counting RR item interactions (streaming events.csv)...")
    rr_counts = _count_rr_item_interactions(events_path, encoding=args.encoding)
    print(f"RR distinct items in events.csv: {len(rr_counts)}")

    mapping = _build_mapping_top_rr_to_products(rr_counts, products)
    print(f"Mapping size (rr_itemid -> product guid): {len(mapping)}")

    if args.out_mapping:
        out_map_path = Path(args.out_mapping).expanduser().resolve()
        _write_mapping_csv(mapping, out_map_path)
        print(f"Wrote mapping CSV: {out_map_path}")

    if args.dry_run:
        print("Dry-run enabled. No DB inserts performed.")
        return 0

    db_url = args.db_url or os.environ.get("DATABASE_URL")
    if not db_url and not args.db_password:
        print(
            "[warn] No --db-url/DATABASE_URL and empty --db-password. "
            "If your DB requires a password, pass it via --db-password or --db-url."
        )

    print("Connecting to DB...")
    conn = _connect(db_url, args.db_host, args.db_port, args.db_name, args.db_user, args.db_password)
    conn.autocommit = False

    insert_sql = """
        INSERT INTO "Tracking"."events_raw"
            ("Id","Ts","Type","Source","UserId","AnonymousId","SessionId","Context","Payload","ReceivedAt")
        VALUES %s
    """

    total = 0
    batch: List[Tuple] = []
    now_received = datetime.now(tz=timezone.utc)

    def flush(cur) -> None:
        nonlocal batch
        if not batch:
            return
        execute_values(
            cur,
            insert_sql,
            batch,
            template="(%s,%s,%s,%s::rec.event_source,%s,%s,%s,%s::jsonb,%s::jsonb,%s)",
            page_size=len(batch),
        )
        batch = []

    try:
        with conn.cursor() as cur:
            for row in _iter_events(events_path, encoding=args.encoding):
                if args.limit and total >= args.limit:
                    break

                rr_event = (row.get("event") or "").strip()
                mapped = _map_event(rr_event)
                if mapped is None:
                    continue
                event_type, source_label = mapped

                rr_item_raw = (row.get("itemid") or "").strip()
                visitor_raw = (row.get("visitorid") or "").strip()
                ts_raw = (row.get("timestamp") or "").strip()
                transaction_raw = (row.get("transactionid") or "").strip()

                rr_itemid = _parse_int(rr_item_raw)
                if rr_itemid is None:
                    continue

                product_guid = mapping.get(rr_itemid)
                if not product_guid:
                    # RR item not mapped (we mapped only top-N)
                    continue

                ts_ms = _parse_int(ts_raw)
                if ts_ms is None:
                    continue

                ts_dt = _to_dt_from_ms(ts_ms)
                user_id = f"rr:{visitor_raw}" if visitor_raw else None

                context = {
                    "source": "Import",
                    "dataset": "RetailRocket",
                    "rr_event": rr_event.lower(),
                }

                payload = {
                    "item_id": product_guid,
                    "rr_itemid": rr_itemid,
                }

                if event_type == "cart_item_added":
                    payload["quantity"] = 1
                elif event_type == "order_placed":
                    # Keep minimal shape but include order_id for computed column OrderId
                    if transaction_raw:
                        payload["order_id"] = str(transaction_raw)
                        payload["transaction_id"] = str(transaction_raw)

                event_id = str(uuid.uuid4())

                # IMPORTANT: Source is enum rec.event_source with labels 'frontend'/'backend'
                batch.append(
                    (
                        event_id,
                        ts_dt,
                        event_type,
                        source_label,
                        user_id,
                        None,  # AnonymousId
                        None,  # SessionId
                        Json(context).dumps(context),
                        Json(payload).dumps(payload),
                        now_received,
                    )
                )
                total += 1

                if len(batch) >= args.batch_size:
                    flush(cur)
                    conn.commit()

            flush(cur)
            conn.commit()

        print(f"Inserted events: {total}")
        return 0
    except Exception:
        conn.rollback()
        raise
    finally:
        conn.close()


if __name__ == "__main__":
    raise SystemExit(main())


