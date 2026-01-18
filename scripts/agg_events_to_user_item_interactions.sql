-- Aggregacja z tabeli online: Tracking.events_raw do tabeli na zagregowane dane -> Tracking.user_item_interactions
-- powstanie macierz rzadka user×item
BEGIN;

WITH agg AS (
    SELECT
        COALESCE(
            "UserId",
            CASE WHEN "AnonymousId" IS NOT NULL THEN 'anon:' || "AnonymousId"::text ELSE NULL END
        )                                                        AS "UserKey",
        "ItemId"                                                  AS "ItemId",
        SUM(CASE WHEN "Type" = 'product_viewed'  THEN 1 ELSE 0 END) AS "Views",
        SUM(CASE WHEN "Type" = 'product_clicked' THEN 1 ELSE 0 END) AS "Clicks",
        SUM(CASE WHEN "Type" = 'cart_item_added' THEN 1 ELSE 0 END) AS "Carts",
        SUM(CASE WHEN "Type" IN ('order_placed','order_paid') THEN 1 ELSE 0 END) AS "Purchases",
        MAX("Ts")                                                 AS "LastTs"
    FROM "Tracking"."events_raw"
    WHERE
        "ItemId" IS NOT NULL
        AND (
            "UserId" IS NOT NULL
            OR "AnonymousId" IS NOT NULL
        )
        AND "Type" IN ('product_viewed','product_clicked','cart_item_added','order_placed','order_paid')
    GROUP BY 1, 2
)
INSERT INTO "Tracking"."user_item_interactions"
    ("UserKey","ItemId","Views","Clicks","Carts","Purchases","Weight","LastTs")
SELECT
    a."UserKey",
    a."ItemId",
    a."Views",
    a."Clicks",
    a."Carts",
    a."Purchases",
    (
        a."Views"     * 1.0 +
        a."Clicks"    * 2.0 +
        a."Carts"     * 5.0 +
        a."Purchases" * 10.0
    )::real AS "Weight",
    a."LastTs"
FROM agg a
WHERE a."UserKey" IS NOT NULL
ON CONFLICT ("UserKey","ItemId") DO UPDATE
SET
    "Views"     = "Tracking"."user_item_interactions"."Views"     + EXCLUDED."Views",
    "Clicks"    = "Tracking"."user_item_interactions"."Clicks"    + EXCLUDED."Clicks",
    "Carts"     = "Tracking"."user_item_interactions"."Carts"     + EXCLUDED."Carts",
    "Purchases" = "Tracking"."user_item_interactions"."Purchases" + EXCLUDED."Purchases",
    "LastTs"    = GREATEST("Tracking"."user_item_interactions"."LastTs", EXCLUDED."LastTs"),
    "Weight"    = (
        ("Tracking"."user_item_interactions"."Views"     + EXCLUDED."Views")     * 1.0 +
        ("Tracking"."user_item_interactions"."Clicks"    + EXCLUDED."Clicks")    * 2.0 +
        ("Tracking"."user_item_interactions"."Carts"     + EXCLUDED."Carts")     * 5.0 +
        ("Tracking"."user_item_interactions"."Purchases" + EXCLUDED."Purchases") * 10.0
    )::real;

COMMIT;


