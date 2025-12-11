import { useCallback, useEffect, useMemo, useRef } from "react";
import {
  trackEvent,
  linkIdentity,
  type FrontendEventType,
} from "../api/trackingService";

const ANON_KEY = "rec_anonymous_id";
const SESSION_KEY = "rec_session_id";
const SESSION_TTL_MIN = 30; // minutes

function generateUuid(): string {
  // Simple UUID v4 generator
  return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, (c) => {
    const r = (Math.random() * 16) | 0;
    const v = c === "x" ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
}

function nowIso(): string {
  return new Date().toISOString();
}

function getFromStorage(key: string): string | null {
  try {
    return window.localStorage.getItem(key);
  } catch {
    return null;
  }
}

function setInStorage(key: string, value: string): void {
  try {
    window.localStorage.setItem(key, value);
  } catch {
    // ignore
  }
}

function getSession(): {
  id: string;
  startedAt: string;
  lastSeenAt: string;
} | null {
  const raw = getFromStorage(SESSION_KEY);
  if (!raw) return null;
  try {
    const parsed = JSON.parse(raw) as {
      id: string;
      startedAt: string;
      lastSeenAt: string;
    };
    return parsed;
  } catch {
    return null;
  }
}

function saveSession(session: {
  id: string;
  startedAt: string;
  lastSeenAt: string;
}): void {
  setInStorage(SESSION_KEY, JSON.stringify(session));
}

export function useTracking(userId?: string | null) {
  const anonymousId = useMemo(() => {
    const existing = getFromStorage(ANON_KEY);
    if (existing) return existing;
    const created = generateUuid();
    setInStorage(ANON_KEY, created);
    return created;
  }, []);

  const session = useMemo(() => {
    const current = getSession();
    const now = Date.now();
    if (!current) {
      const created = {
        id: generateUuid(),
        startedAt: nowIso(),
        lastSeenAt: nowIso(),
      };
      saveSession(created);
      return created;
    }

    const lastSeen = new Date(current.lastSeenAt).getTime();
    const minutesSinceLastSeen = (now - lastSeen) / (1000 * 60);

    if (minutesSinceLastSeen > SESSION_TTL_MIN) {
      const fresh = {
        id: generateUuid(),
        startedAt: nowIso(),
        lastSeenAt: nowIso(),
      };
      saveSession(fresh);
      return fresh;
    }

    const updated = { ...current, lastSeenAt: nowIso() };
    saveSession(updated);
    return updated;
  }, []);

  const sessionHeartbeatRef = useRef<number | null>(null);

  useEffect(() => {
    // Keep session alive
    sessionHeartbeatRef.current = window.setInterval(() => {
      const current = getSession();
      if (!current) return;
      const updated = { ...current, lastSeenAt: nowIso() };
      saveSession(updated);
    }, 60_000);

    return () => {
      if (sessionHeartbeatRef.current)
        window.clearInterval(sessionHeartbeatRef.current);
    };
  }, []);

  useEffect(() => {
    if (userId) {
      // Link identities in backend so we can merge anon history
      void linkIdentity(anonymousId, userId);
    }
  }, [anonymousId, userId]);

  const emit = useCallback(
    async (
      eventType: FrontendEventType | string,
      payload?: Record<string, unknown> | null,
      context?: Record<string, unknown> | null
    ) => {
      const baseContext = {
        page_url: window.location.href,
        referrer: document.referrer || undefined,
        user_agent: navigator.userAgent,
        viewport: { w: window.innerWidth, h: window.innerHeight },
        ...context,
      } as Record<string, unknown>;

      return await trackEvent({
        eventType,
        source: "frontend",
        userId: userId ?? undefined,
        anonymousId,
        sessionId: session.id,
        context: baseContext,
        payload: payload ?? undefined,
      });
    },
    [anonymousId, session.id, userId]
  );

  // Helpers
  const productViewed = useCallback(
    async (itemId: string, dwellMs?: number) =>
      emit("product_viewed", { item_id: itemId, dwell_ms: dwellMs }),
    [emit]
  );

  const productClicked = useCallback(
    async (itemId: string, listId?: string, position?: number) =>
      emit(
        "product_clicked",
        { item_id: itemId, position },
        listId ? { list_id: listId } : undefined
      ),
    [emit]
  );

  const itemImpression = useCallback(
    async (itemId: string, listId?: string, position?: number) =>
      emit(
        "item_impression",
        { item_id: itemId, position },
        listId ? { list_id: listId } : undefined
      ),
    [emit]
  );

  const listViewed = useCallback(
    async (listId: string, visibleItemIds?: string[]) =>
      emit("list_viewed", { items: visibleItemIds }, { list_id: listId }),
    [emit]
  );

  const recImpression = useCallback(
    async (
      widgetId: string,
      algo: string,
      seedItemId: string,
      itemId: string,
      position: number
    ) =>
      emit(
        "rec_impression",
        { item_id: itemId, position },
        { widget_id: widgetId, algo, seed_item_id: seedItemId }
      ),
    [emit]
  );

  const recClick = useCallback(
    async (
      widgetId: string,
      algo: string,
      seedItemId: string,
      itemId: string,
      position: number
    ) =>
      emit(
        "rec_click",
        { item_id: itemId, position },
        { widget_id: widgetId, algo, seed_item_id: seedItemId }
      ),
    [emit]
  );

  const searchPerformed = useCallback(
    async (
      query: string,
      resultCount: number,
      filters?: Record<string, unknown>
    ) =>
      emit("search_performed", { query, result_count: resultCount, filters }),
    [emit]
  );

  const checkoutStarted = useCallback(
    async () => emit("checkout_started"),
    [emit]
  );

  return {
    anonymousId,
    sessionId: session.id,
    emit,
    productViewed,
    productClicked,
    itemImpression,
    listViewed,
    recImpression,
    recClick,
    searchPerformed,
    checkoutStarted,
  } as const;
}
