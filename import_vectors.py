#!/usr/bin/env python3
"""
Skrypt do importu wektorów produktów z plików PKL/CSV do bazy danych PostgreSQL z pgvector.
"""

import json
import sys
import os
import argparse
from typing import List, Dict, Any
import psycopg2
from psycopg2.extras import RealDictCursor
import pandas as pd
import numpy as np
from tqdm import tqdm

# Konfiguracja bazy danych
DB_CONFIG = {
    'host': '192.168.1.245',
    'port': 5432,
    'database': 'Recommendations',
    'user': 'someuser',
    'password': 'somepassword'
}

# Mapowanie nazw plików na warianty
VARIANT_MAPPING = {
    'product_vectors': 'Full',
    'product_vectors_no_brand': 'NoBrand', 
    'product_vectors_no_brand_and_attributes': 'NoBrandAndAttributes',
    'product_vectors_only_description': 'OnlyDescription'
}

def connect_to_db():
    """Nawiązuje połączenie z bazą danych."""
    try:
        conn = psycopg2.connect(**DB_CONFIG)
        return conn
    except Exception as e:
        print(f"Błąd połączenia z bazą danych: {e}")
        sys.exit(1)

def create_table_if_not_exists(conn):
    """Tworzy tabelę ProductEmbeddings jeśli nie istnieje."""
    with conn.cursor() as cur:
        # Sprawdź czy rozszerzenie vector jest dostępne
        cur.execute("SELECT 1 FROM pg_extension WHERE extname = 'vector'")
        if not cur.fetchone():
            print("Rozszerzenie 'vector' nie jest zainstalowane!")
            sys.exit(1)
        
        # Sprawdź czy schemat Vectors istnieje
        cur.execute("SELECT 1 FROM information_schema.schemata WHERE schema_name = 'Vectors'")
        if not cur.fetchone():
            cur.execute("CREATE SCHEMA IF NOT EXISTS \"Vectors\"")
        
        # Sprawdź czy tabela istnieje
        cur.execute("""
            SELECT 1 FROM information_schema.tables 
            WHERE table_schema = 'Vectors' AND table_name = 'ProductEmbeddings'
        """)
        if not cur.fetchone():
            print("Tabela ProductEmbeddings nie istnieje! Uruchom migrację EF Core.")
            sys.exit(1)
        
        conn.commit()

def parse_vector_string(vector_str: str) -> List[float]:
    """Parsuje string wektora na listę floatów."""
    try:
        # Usuń nawiasy kwadratowe i podziel po przecinkach
        vector_str = vector_str.strip('[]')
        values = [float(x.strip()) for x in vector_str.split(',')]
        
        if len(values) != 768:
            raise ValueError(f"Wektor ma {len(values)} elementów, oczekiwano 768")
        
        return values
    except Exception as e:
        raise ValueError(f"Błąd parsowania wektora: {e}")

def import_from_csv(conn, csv_file: str, variant: str, batch_size: int = 1000):
    """Importuje wektory z pliku CSV."""
    print(f"Importuję {variant} z {csv_file}...")
    
    # Wczytaj CSV
    df = pd.read_csv(csv_file)
    print(f"Wczytano {len(df)} wierszy")
    
    # Sprawdź kolumny
    if 'id' not in df.columns or 'vector' not in df.columns:
        print("Błąd: Plik CSV musi mieć kolumny 'id' i 'vector'")
        return
    
    with conn.cursor() as cur:
        # Przygotuj zapytanie
        insert_query = """
            INSERT INTO "Vectors"."ProductEmbeddings" ("ProductId", "Variant", "Embedding", "CreatedAt")
            SELECT p."Id", %s, %s, NOW()
            FROM "Dictionary"."Products" p
            WHERE p."ExternalId" = %s
            ON CONFLICT ("ProductId", "Variant") 
            DO UPDATE SET 
                "Embedding" = EXCLUDED."Embedding",
                "UpdatedAt" = NOW()
        """
        
        # Przetwarzaj w batchach
        for i in tqdm(range(0, len(df), batch_size), desc=f"Import {variant}"):
            batch = df.iloc[i:i+batch_size]
            batch_data = []
            
            for _, row in batch.iterrows():
                try:
                    product_id = str(row['id'])  
                    vector_values = parse_vector_string(row['vector'])
                    
                    # Konwertuj na format pgvector
                    vector_str = f"[{','.join(map(str, vector_values))}]"
                    
                    batch_data.append((product_id, variant, vector_str))
                    
                except Exception as e:
                    print(f"Błąd w wierszu {row.name}: {e}")
                    continue
            
            if batch_data:
                try:
                    cur.executemany(insert_query, batch_data)
                    conn.commit()
                except Exception as e:
                    print(f"Błąd zapisu batch {i//batch_size}: {e}")
                    conn.rollback()

def import_from_pkl(conn, pkl_file: str, variant: str, batch_size: int = 1000):
    """Importuje wektory z pliku PKL."""
    print(f"Importuję {variant} z {pkl_file}...")
    
    try:
        # Wczytaj PKL
        data = pd.read_pickle(pkl_file)
        print(f"Wczytano dane z PKL: {type(data)}")
        
        # Sprawdź format danych
        if isinstance(data, dict):
            # Zakładamy format {id: vector, ...}
            items = data.items()
        elif isinstance(data, pd.DataFrame):
            # Zakładamy DataFrame z kolumnami 'id' i 'vector'
            items = zip(data['id'], data['vector'])
        else:
            print(f"Nieobsługiwany format danych: {type(data)}")
            return
        
        with conn.cursor() as cur:
            insert_query = """
                INSERT INTO "Vectors"."ProductEmbeddings" ("ProductId", "Variant", "Embedding", "CreatedAt")
                SELECT p."Id", %s, %s, NOW()
                FROM "Dictionary"."Products" p
                WHERE p."ExternalId" = %s
                ON CONFLICT ("ProductId", "Variant") 
                DO UPDATE SET 
                    "Embedding" = EXCLUDED."Embedding",
                    "UpdatedAt" = NOW()
            """
            
            batch_data = []
            count = 0
            
            for product_id, vector in tqdm(items, desc=f"Import {variant}"):
                try:
                    # Konwertuj product_id na string
                    product_id_str = str(product_id)
                    
                    # Sprawdź format wektora
                    if isinstance(vector, (list, np.ndarray)):
                        vector_values = list(vector)
                    elif isinstance(vector, str):
                        vector_values = parse_vector_string(vector)
                    else:
                        print(f"Nieobsługiwany format wektora dla ID {product_id}: {type(vector)}")
                        continue
                    
                    if len(vector_values) != 768:
                        print(f"Wektor dla ID {product_id} ma {len(vector_values)} elementów, oczekiwano 768")
                        continue
                    
                    # Konwertuj na format pgvector
                    vector_str = f"[{','.join(map(str, vector_values))}]"
                    
                    batch_data.append((variant, vector_str, product_id_str))
                    count += 1
                    
                    # Zapisz batch
                    if len(batch_data) >= batch_size:
                        try:
                            cur.executemany(insert_query, batch_data)
                            conn.commit()
                            batch_data = []
                        except Exception as e:
                            print(f"Błąd zapisu batch: {e}")
                            conn.rollback()
                            batch_data = []
                    
                except Exception as e:
                    print(f"Błąd w ID {product_id}: {e}")
                    continue
            
            # Zapisz pozostałe dane
            if batch_data:
                try:
                    cur.executemany(insert_query, batch_data)
                    conn.commit()
                except Exception as e:
                    print(f"Błąd zapisu ostatniego batch: {e}")
                    conn.rollback()
            
            print(f"Zaimportowano {count} wektorów dla wariantu {variant}")
            
    except Exception as e:
        print(f"Błąd wczytywania pliku PKL {pkl_file}: {e}")

def main():
    parser = argparse.ArgumentParser(description='Import wektorów produktów do bazy danych')
    parser.add_argument('--data-dir', required=True, help='Katalog z plikami wektorów')
    parser.add_argument('--format', choices=['csv', 'pkl', 'both'], default='csv', 
                       help='Format plików do importu (domyślnie csv)')
    parser.add_argument('--batch-size', type=int, default=1000, 
                       help='Rozmiar batch dla importu')
    parser.add_argument('--host', default='localhost', help='Host bazy danych')
    parser.add_argument('--port', type=int, default=5432, help='Port bazy danych')
    parser.add_argument('--database', default='recommendations', help='Nazwa bazy danych')
    parser.add_argument('--user', default='postgres', help='Użytkownik bazy danych')
    parser.add_argument('--password', default='postgres', help='Hasło bazy danych')
    
    args = parser.parse_args()
    
    # Aktualizuj konfigurację DB
    DB_CONFIG.update({
        'host': args.host,
        'port': args.port,
        'database': args.database,
        'user': args.user,
        'password': args.password
    })
    
    # Sprawdź czy katalog istnieje
    if not os.path.exists(args.data_dir):
        print(f"Katalog {args.data_dir} nie istnieje!")
        sys.exit(1)
    
    # Połącz z bazą danych
    conn = connect_to_db()
    create_table_if_not_exists(conn)
    
    try:
        # Importuj każdy wariant
        for file_prefix, variant in VARIANT_MAPPING.items():
            print(f"\n=== Import wariantu {variant} ===")
            
            if args.format in ['csv', 'both']:
                csv_file = os.path.join(args.data_dir, f"{file_prefix}.csv")
                if os.path.exists(csv_file):
                    import_from_csv(conn, csv_file, variant, args.batch_size)
                else:
                    print(f"Plik CSV {csv_file} nie istnieje")
            
            if args.format in ['pkl', 'both']:
                pkl_file = os.path.join(args.data_dir, f"{file_prefix}.pkl")
                if os.path.exists(pkl_file):
                    import_from_pkl(conn, pkl_file, variant, args.batch_size)
                else:
                    print(f"Plik PKL {pkl_file} nie istnieje")
        
        print("\n=== Import zakończony ===")
        
        # Pokaż statystyki
        with conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT "Variant", COUNT(*) as count 
                FROM "Vectors"."ProductEmbeddings" 
                GROUP BY "Variant"
            """)
            stats = cur.fetchall()
            
            print("\nStatystyki:")
            for stat in stats:
                print(f"  {stat['variant']}: {stat['count']} wektorów")
        
    finally:
        conn.close()

if __name__ == "__main__":
    main()
