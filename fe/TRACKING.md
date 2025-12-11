# System Trackingu i Zbierania Danych dla Rekomendacji

## Przegląd

System trackingu w module **Recommendations.Tracking** zbiera i przetwarza zdarzenia użytkowników z backendu i frontendu w celu:

- Budowania profili interakcji użytkownik–produkt
- Trenowania modeli rekomendacji (Collaborative Filtering, Online Learning)
- Ewaluacji skuteczności algorytmów rekomendacji
- Analizy zachowań użytkowników

System obsługuje zarówno użytkowników zalogowanych (`userId`), jak i anonimowych gości (`anonymousId`), z możliwością linkowania tożsamości po zalogowaniu.

---

## Architektura Backendu

### Baza Danych – Tabele i Ich Przeznaczenie

System wykorzystuje dwa konteksty bazy danych: **TrackingDbContext** (surowe eventy) i **SignalsDbContext** (przetworzone sygnały i embeddingi).

#### 1. `events_raw` (TrackingDbContext)

**Przeznaczenie:** Surowy log wszystkich zdarzeń z backendu i frontendu. To główna tabela append-only dla wszystkich eventów, z elastycznym schematem JSON dla `context` i `payload`.

**Schemat:**

- `Id` (uuid, PK)
- `Ts` (timestamptz) – znacznik czasu zdarzenia (UTC)
- `Type` (text) – typ zdarzenia, np. `order_placed`, `product_viewed`
- `Source` (enum: `frontend`/`backend`)
- `UserId` (text, nullable, indeksowane) – ID zalogowanego użytkownika
- `AnonymousId` (uuid, nullable, indeksowane) – ID anonimowego gościa
- `SessionId` (uuid, nullable) – ID sesji frontendowej
- `Context` (jsonb) – kontekst zdarzenia (moduł, strona, widget, algoritm, user-agent, itp.)
- `Payload` (jsonb) – dane specyficzne dla zdarzenia (item_id, order_id, quantity, filters, itp.)
- `ReceivedAt` (timestamptz) – kiedy event dotarł do backendu
- **`ItemId` (text, computed, stored, indeksowane)** – wyciągnięte z `Payload->>'item_id'`
- **`OrderId` (text, computed, stored, indeksowane)** – wyciągnięte z `Payload->>'order_id'`

**Indeksy:**

- `(Type, Ts)` – szybkie zapytania o eventy danego typu w zakresie czasu
- `UserId`, `AnonymousId`, `ItemId`, `OrderId` – do joinów i filtrowania

**Użycie:** Wszystkie zdarzenia (zakupy, koszyk, oglądanie produktów, kliknięcia w rekomendacje) trafiają tutaj. Dane są później agregowane do `user_item_interactions`.

---

#### 2. `identity_links` (TrackingDbContext)

**Przeznaczenie:** Mapowanie `AnonymousId <-> UserId` po zalogowaniu, umożliwiające scalenie historii anonimowej z profilem użytkownika.

**Schemat:**

- `AnonymousId` (uuid, PK część 1)
- `UserId` (text, PK część 2)
- `LinkedAt` (timestamptz)

**Użycie:** Gdy użytkownik anonimowy się loguje, wywołujemy `LinkIdentity(anonymousId, userId)`. Dzięki temu możemy przypisać jego wcześniejsze interakcje (np. oglądanie produktów przed logowaniem) do jego konta.

---

#### 3. `events_rejected` (TrackingDbContext)

**Przeznaczenie:** Przechowuje eventy, które nie przeszły walidacji lub parsowania, wraz z powodem odrzucenia. Służy do monitorowania jakości danych.

**Schemat:**

- `Id` (uuid, PK)
- `ReceivedAt` (timestamptz)
- `Reason` (text) – powód odrzucenia
- `Raw` (jsonb) – surowe dane eventu

**Użycie:** Monitoring błędów integracji, wykrywanie problemów z danymi.

---

#### 4. `user_item_interactions` (SignalsDbContext)

**Przeznaczenie:** Zagregowane sygnały interakcji użytkownik–produkt. To tabela **feature store** dla modeli rekomendacji.

**Schemat:**

- `UserKey` (text, PK część 1) – `userId` lub `anon:<anonymousId>`
- `ItemId` (text, PK część 2)
- `Views` (int) – liczba obejrzeń produktu
- `Clicks` (int) – liczba kliknięć w produkt na listach
- `Carts` (int) – liczba dodań do koszyka
- `Purchases` (int) – liczba zakupów
- `Weight` (float) – waga interakcji (np. time-decay + typ zdarzenia)
- `LastTs` (timestamptz, indeksowane)

**Użycie:** Agregacja z `events_raw` (batch lub stream). Wykorzystywane przez:

- Collaborative Filtering (budowanie macierzy user–item)
- Online Learning (aktualizacja embeddingów użytkownika)
- Ranking i personalizacja

---

#### 5. `user_embeddings_online` (SignalsDbContext)

**Przeznaczenie:** Wektory użytkowników aktualizowane w czasie rzeczywistym (online learning) na podstawie najnowszych interakcji.

**Schemat:**

- `UserKey` (text, PK)
- `Emb` (vector(768)) – pgvector, embedding użytkownika
- `UpdatedAt` (timestamptz, indeksowane)

**Użycie:** Real-time rekomendacje – po nowej interakcji użytkownika (np. obejrzeniu produktu) jego embedding jest aktualizowany i używany do szybkiego wyszukiwania podobnych produktów.

---

#### 6. `user_embeddings_cf` i `item_embeddings_cf` (SignalsDbContext)

**Przeznaczenie:** Wektory użytkowników i produktów wytrenowane offline (Collaborative Filtering batch).

**Schemat:**

- `UserKey` / `ItemId` (text, PK)
- `Emb` (vector(768))
- `ModelVer` (text) – wersja modelu
- `TrainedAt` (timestamptz, indeksowane)

**Użycie:** Okresowy (np. nocny) retraining modelu CF na całej historii `user_item_interactions`. Embeddingi są używane do rekomendacji typu "users who bought this also bought" i "similar users".

---

### Eventy Backendowe

Backend emituje eventy wewnętrznie (DomainEvents) i zapisuje je do `events_raw` przez event handlery. **Nie duplikujemy tych eventów na frontendzie.**

#### `order_placed`

- **Kiedy:** Po złożeniu zamówienia (`OrderPlaced` event z modułu Purchase)
- **Source:** `backend`
- **Context:** `{ source: "Backend", module: "Purchase", action: "CreateOrder" }`
- **Payload:**
  ```json
  {
    "order_id": "abc-123",
    "total_amount": 499.99,
    "items_count": 3,
    "items": [
      { "item_id": "123", "quantity": 2, "price": 149.99 },
      { "item_id": "456", "quantity": 1, "price": 200.01 }
    ]
  }
  ```

#### `order_paid`

- **Kiedy:** Po opłaceniu zamówienia (`OrderPaid` event)
- **Source:** `backend`
- **Context:** `{ source: "Backend", module: "Purchase", action: "PayForOrder" }`
- **Payload:**
  ```json
  {
    "order_id": "abc-123",
    "total_amount": 499.99,
    "payment_method": "CreditCard"
  }
  ```

#### Eventy Koszyka (z modułu Cart)

- `cart_item_added` – dodanie produktu do koszyka
- `cart_item_removed` – usunięcie produktu z koszyka
- `cart_item_quantity_changed` – zmiana ilości
- `cart_cleared` – wyczyszczenie koszyka

**Payload przykład:**

```json
{
  "item_id": "123",
  "quantity": 2,
  "user_id": "user-456"
}
```

---

## Architektura Frontendu

### Struktura Kodu

```
fe/src/
├── api/
│   └── trackingService.tsx        # API client dla /tracking endpoint
├── hooks/
│   └── useTracking.ts             # Hook zarządzający sessionId, anonymousId i emitowaniem eventów
├── components/
│   ├── ProductCard.tsx            # Instrumentacja: product_clicked, item_impression
│   └── SimilarProducts.tsx        # Instrumentacja: rec_impression, rec_click
└── pages/
    ├── ProductPage.tsx            # Instrumentacja: product_viewed z dwell time
    ├── ProductsPage.tsx           # Instrumentacja: list_viewed, item_impression
    └── CategoryPage.tsx           # Instrumentacja: list_viewed, item_impression
```

---

### Hook: `useTracking(userId?)`

**Funkcjonalność:**

- Generuje i persystuje `anonymousId` (localStorage, `rec_anonymous_id`)
- Zarządza sesją użytkownika (localStorage, `rec_session_id`) z TTL 30 min
- Automatycznie linkuje `anonymousId <-> userId` po zalogowaniu
- Zapewnia helper functions do emitowania eventów z automatycznym kontekstem (page_url, referrer, user_agent, viewport)

**API:**

```typescript
const {
  anonymousId,
  sessionId,
  emit, // (eventType, payload?, context?) => Promise<ApiResult<string>>
  productViewed, // (itemId, dwellMs?) => Promise
  productClicked, // (itemId, listId?, position?) => Promise
  itemImpression, // (itemId, listId?, position?) => Promise
  listViewed, // (listId, visibleItemIds?) => Promise
  recImpression, // (widgetId, algo, seedItemId, itemId, position) => Promise
  recClick, // (widgetId, algo, seedItemId, itemId, position) => Promise
  searchPerformed, // (query, resultCount, filters?) => Promise
  checkoutStarted, // () => Promise
} = useTracking(currentUserId);
```

---

### Eventy Frontendowe

**Zasady:**

- `source` zawsze: `"frontend"`
- `context` zawsze zawiera: `page_url`, `referrer`, `user_agent`, `viewport`
- `payload` i `context` używają **snake_case** (zgodnie z konwencją backendu)
- **Nie duplikujemy eventów backendowych** (koszyk, zamówienia są emitowane tylko z backendu)

---

#### `product_viewed`

**Kiedy:** Użytkownik widzi stronę produktu (PDP). Event emitowany przy unmount z czasem spędzonym na stronie.

**Payload:**

```json
{
  "item_id": "123",
  "dwell_ms": 18450
}
```

**Użycie:**

```typescript
// ProductPage.tsx
useEffect(() => {
  const startTime = Date.now();
  return () => {
    const dwellMs = Date.now() - startTime;
    void productViewed(id, dwellMs);
  };
}, [id, productViewed]);
```

---

#### `product_clicked`

**Kiedy:** Użytkownik klika w link do produktu na liście (PLP, kategoria, wyszukiwanie).

**Payload:**

```json
{
  "item_id": "123",
  "position": 7
}
```

**Context:**

```json
{
  "list_id": "category:shoes_page:2"
}
```

**Użycie:**

```typescript
// ProductCard.tsx
<Link to={`/product/${product.id}`} onClick={() => productClicked(product.id, listId, position)}>
```

---

#### `item_impression`

**Kiedy:** Produkt został wyrenderowany na liście (viewport impression tracking opcjonalne, obecnie emitujemy przy renderze).

**Payload:**

```json
{
  "item_id": "123",
  "position": 7
}
```

**Context:**

```json
{
  "list_id": "products_page_1"
}
```

**Użycie:**

```typescript
// ProductCard.tsx (automatycznie w useEffect)
useEffect(() => {
  if (listId && typeof position === "number") {
    void itemImpression(String(product.id), listId, position);
  }
}, [itemImpression, listId, position, product.id]);
```

---

#### `rec_impression`

**Kiedy:** Produkt rekomendowany wyświetlony w widżecie rekomendacji.

**Payload:**

```json
{
  "item_id": "456",
  "position": 2
}
```

**Context:**

```json
{
  "widget_id": "pdp_similar",
  "algo": "ContentBasedFull",
  "seed_item_id": "123"
}
```

**Użycie:**

```typescript
// SimilarProducts.tsx
useEffect(() => {
  similarProducts.forEach((p, idx) => {
    void recImpression(
      "pdp_similar",
      algorithm,
      productId,
      String(p.id),
      idx + 1
    );
  });
}, [similarProducts, algorithm, productId, recImpression]);
```

---

#### `rec_click`

**Kiedy:** Użytkownik klika w produkt z widżetu rekomendacji.

**Payload:**

```json
{
  "item_id": "456",
  "position": 2
}
```

**Context:**

```json
{
  "widget_id": "pdp_similar",
  "algo": "ContentBasedFull",
  "seed_item_id": "123"
}
```

**Znaczenie:** Kluczowy sygnał do ewaluacji algorytmów rekomendacji (CTR, conversion z rekomendacji).

---

#### `list_viewed`

**Kiedy:** Załadowanie listy produktów (PLP/kategoria). Opcjonalnie z listą widocznych `item_id`.

**Payload:**

```json
{
  "items": ["123", "456", "789"]
}
```

**Context:**

```json
{
  "list_id": "category:shoes"
}
```

---

#### `search_performed`

**Kiedy:** Wykonanie wyszukiwania w sklepie.

**Payload:**

```json
{
  "query": "buty do biegania",
  "result_count": 142,
  "filters": { "brand": ["Nike"], "price_to": 300 }
}
```

---

#### `checkout_started`

**Kiedy:** Wejście w proces checkout (pierwsza strona checkoutu).

**Payload:** (pusty lub minimalne info)

**Użycie:** Analiza lejków konwersji (checkout funnel).

---

### Linkowanie Tożsamości

**Problem:** Użytkownik przegląda sklep jako gość (generujemy `anonymousId`), ogląda produkty, dodaje do koszyka, potem się loguje. Chcemy połączyć jego historię anonimową z kontem.

**Rozwiązanie:** Tabela `identity_links`.

**Przepływ:**

1. Frontend generuje `anonymousId` przy pierwszej wizycie (localStorage).
2. Użytkownik przegląda sklep → eventy trafiają do `events_raw` z `anonymousId`, `userId = null`.
3. Użytkownik się loguje → frontend wywołuje `linkIdentity(anonymousId, userId)`.
4. Backend zapisuje link w `identity_links`.
5. Późniejsze eventy mają zarówno `userId`, jak i `anonymousId`.
6. Przy agregacji do `user_item_interactions` scalamy interakcje: `anon:<anonymousId>` → `userId`.

**Kod (frontend):**

```typescript
// useTracking.ts
useEffect(() => {
  if (userId) {
    void linkIdentity(anonymousId, userId);
  }
}, [anonymousId, userId]);
```

---

### Zarządzanie Sesjami

**Definicja sesji:** Ciągła aktywność użytkownika z TTL 30 minut (bez aktywności → nowa sesja).

**Implementacja (frontend):**

- `sessionId` generowany przy pierwszym ładowaniu lub po wygaśnięciu TTL
- Session object: `{ id, startedAt, lastSeenAt }` w localStorage
- Heartbeat co 60s aktualizuje `lastSeenAt`
- Przy każdym evencie sprawdzamy: jeśli `lastSeenAt` > 30 min → nowa sesja

**Kod:**

```typescript
// useTracking.ts
const session = useMemo(() => {
  const current = getSession();
  const now = Date.now();
  if (!current) return createSession();

  const minutesSinceLastSeen =
    (now - new Date(current.lastSeenAt).getTime()) / (1000 * 60);
  if (minutesSinceLastSeen > SESSION_TTL_MIN) return createSession();

  return updateSession(current);
}, []);
```

**Użycie w analizie:**

- Długość sesji
- Liczba produktów na sesję
- Konwersja per sesja
- Session-based rekomendacje

---

## Wykorzystanie Danych do Trenowania Modelu

### Pipeline Przetwarzania

```
events_raw (surowe eventy)
    ↓
[ETL / Stream Processing]
    ↓
user_item_interactions (agregowane sygnały)
    ↓
[Trening Modelu CF Batch]
    ↓
user_embeddings_cf + item_embeddings_cf
    ↓
[Inference: rekomendacje offline]

events_raw (nowe interakcje)
    ↓
[Online Learning]
    ↓
user_embeddings_online
    ↓
[Inference: rekomendacje real-time]
```

### Mapowanie Eventów na Interakcje

| Event Type        | Field Updated        | Weight Multiplier |
| ----------------- | -------------------- | ----------------- |
| `product_viewed`  | `Views++`            | 1.0               |
| `product_clicked` | `Clicks++`           | 2.0               |
| `item_impression` | (opcjonalne Views++) | 0.1               |
| `cart_item_added` | `Carts++`            | 5.0               |
| `order_placed`    | `Purchases++`        | 10.0              |
| `rec_impression`  | (ewaluacja algo)     | -                 |
| `rec_click`       | (ewaluacja algo)     | -                 |

**Weight calculation example:**

```
Weight = (Views * 1.0 + Clicks * 2.0 + Carts * 5.0 + Purchases * 10.0) * time_decay_factor
```

**Time decay:** Nowsze interakcje mają wyższą wagę (np. exponential decay).

---

### Agregacja: `events_raw` → `user_item_interactions`

**SQL pseudokod:**

```sql
INSERT INTO user_item_interactions (UserKey, ItemId, Views, Clicks, Carts, Purchases, Weight, LastTs)
SELECT
  COALESCE(UserId, 'anon:' || AnonymousId::text) AS UserKey,
  Payload->>'item_id' AS ItemId,
  SUM(CASE WHEN Type = 'product_viewed' THEN 1 ELSE 0 END) AS Views,
  SUM(CASE WHEN Type = 'product_clicked' THEN 1 ELSE 0 END) AS Clicks,
  SUM(CASE WHEN Type = 'cart_item_added' THEN 1 ELSE 0 END) AS Carts,
  SUM(CASE WHEN Type IN ('order_placed', 'order_paid') THEN JSON_ARRAY_LENGTH(...) ELSE 0 END) AS Purchases,
  -- calculate Weight
  MAX(Ts) AS LastTs
FROM events_raw
WHERE ItemId IS NOT NULL
GROUP BY UserKey, ItemId
ON CONFLICT (UserKey, ItemId) DO UPDATE
  SET Views = user_item_interactions.Views + EXCLUDED.Views,
      Clicks = user_item_interactions.Clicks + EXCLUDED.Clicks,
      ...
      LastTs = GREATEST(user_item_interactions.LastTs, EXCLUDED.LastTs);
```

**Uruchamianie:**

- Batch: cron job co godzinę/co noc
- Stream: Kafka/RabbitMQ consumer w czasie rzeczywistym

---

### Trening Modelu

**Collaborative Filtering (Offline):**

1. Eksportuj `user_item_interactions` do formatu treningowego (macierz rzadka user×item)
2. Trenuj model (np. Matrix Factorization, Neural CF)
3. Zapisz embeddingi do `user_embeddings_cf` i `item_embeddings_cf`
4. W aplikacji: wyszukiwanie najbliższych sąsiadów (pgvector `<->` operator)

**Online Learning:**

1. Przy nowej interakcji użytkownika: aktualizuj `user_embeddings_online`
2. Użyj prostego modelu (np. średnia ważona embeddingów produktów, które użytkownik widział)
3. Real-time inference: `SELECT * FROM item_embeddings_cf ORDER BY Emb <-> user_emb LIMIT 10`

---

## Ewaluacja Algorytmów Rekomendacji

**Metryki z eventów `rec_impression` i `rec_click`:**

- **CTR (Click-Through Rate):**

  ```
  CTR = rec_click_count / rec_impression_count
  ```

- **Conversion Rate:**
  Ile produktów z `rec_click` zostało zakupionych (join `rec_click` → `order_placed`)?

- **A/B Testing:**
  - Context zawiera `algo` → możemy porównać różne algorytmy
  - Np. `ContentBasedFull` vs `ContentBasedNoBrand` na podstawie CTR, conversion

**Przykładowe zapytanie:**

```sql
SELECT
  Context->>'algo' AS algorithm,
  COUNT(*) FILTER (WHERE Type = 'rec_impression') AS impressions,
  COUNT(*) FILTER (WHERE Type = 'rec_click') AS clicks,
  ROUND(COUNT(*) FILTER (WHERE Type = 'rec_click')::numeric /
        NULLIF(COUNT(*) FILTER (WHERE Type = 'rec_impression'), 0), 4) AS ctr
FROM events_raw
WHERE Type IN ('rec_impression', 'rec_click')
  AND Context->>'widget_id' = 'pdp_similar'
  AND Ts > NOW() - INTERVAL '7 days'
GROUP BY algorithm;
```

---

## Podsumowanie

### Backend

- **3 tabele tracking:** `events_raw` (append-only log), `identity_links` (anonimowy↔zalogowany), `events_rejected` (monitoring)
- **4 tabele signals:** `user_item_interactions` (feature store), `user_embeddings_online`, `user_embeddings_cf`, `item_embeddings_cf`
- **Eventy backendowe:** `order_placed`, `order_paid`, `cart_*` – automatycznie emitowane przez domain events

### Frontend

- **Hook `useTracking`:** zarządza `anonymousId`, `sessionId`, linkuje tożsamości, emituje eventy
- **Service `trackingService`:** API client dla `/tracking/track` i `/tracking/link-identity`
- **Instrumentacja:**
  - `ProductCard`: `product_clicked`, `item_impression`
  - `SimilarProducts`: `rec_impression`, `rec_click`
  - `ProductPage`: `product_viewed` (z dwell time)
  - `ProductsPage`, `CategoryPage`: automatyczne impressions przez `ProductCard`

### Dane do ML

- **Agregacja:** `events_raw` → `user_item_interactions` (batch/stream)
- **Trening CF:** batch job → embeddingi do `*_embeddings_cf`
- **Online Learning:** real-time update → `user_embeddings_online`
- **Ewaluacja:** `rec_impression`/`rec_click` → CTR, conversion per algorytm

---

## Przykłady Wykorzystania

### 1. Analiza Popularności Produktów

```sql
SELECT
  ItemId,
  COUNT(*) FILTER (WHERE Type = 'product_viewed') AS views,
  COUNT(*) FILTER (WHERE Type = 'product_clicked') AS clicks,
  COUNT(*) FILTER (WHERE Type = 'cart_item_added') AS carts
FROM events_raw
WHERE Ts > NOW() - INTERVAL '30 days'
GROUP BY ItemId
ORDER BY views DESC
LIMIT 20;
```

### 2. Lejek Konwersji (Funnel)

```sql
WITH funnel AS (
  SELECT
    SessionId,
    MAX(CASE WHEN Type = 'product_viewed' THEN 1 ELSE 0 END) AS viewed,
    MAX(CASE WHEN Type = 'cart_item_added' THEN 1 ELSE 0 END) AS carted,
    MAX(CASE WHEN Type = 'checkout_started' THEN 1 ELSE 0 END) AS checkout,
    MAX(CASE WHEN Type = 'order_placed' THEN 1 ELSE 0 END) AS purchased
  FROM events_raw
  WHERE Ts > NOW() - INTERVAL '7 days'
  GROUP BY SessionId
)
SELECT
  SUM(viewed) AS total_sessions,
  SUM(carted) AS added_to_cart,
  SUM(checkout) AS started_checkout,
  SUM(purchased) AS completed_purchase,
  ROUND(SUM(carted)::numeric / NULLIF(SUM(viewed), 0), 4) AS cart_rate,
  ROUND(SUM(purchased)::numeric / NULLIF(SUM(carted), 0), 4) AS conversion_rate
FROM funnel;
```

### 3. Top Rekomendowane Produkty (które użytkownicy klikają)

```sql
SELECT
  Payload->>'item_id' AS recommended_item,
  Context->>'algo' AS algorithm,
  COUNT(*) AS click_count
FROM events_raw
WHERE Type = 'rec_click'
  AND Ts > NOW() - INTERVAL '7 days'
GROUP BY recommended_item, algorithm
ORDER BY click_count DESC
LIMIT 10;
```

---

**Autorzy:** System zbudowany w ramach pracy magisterskiej – moduł Tracking & Recommendations  
**Data ostatniej aktualizacji:** 2025-10-20
