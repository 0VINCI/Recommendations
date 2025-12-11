import { post } from "./client/httpClient.tsx";
import type { ApiResult } from "../types/api/ApiResult.tsx";

export type FrontendEventType =
  | "product_viewed"
  | "item_impression"
  | "product_clicked"
  | "list_viewed"
  | "search_performed"
  | "rec_impression"
  | "rec_click"
  | "checkout_started"
  | "session_start"
  | "session_end";

export interface TrackEventCommand {
  eventType: FrontendEventType | string;
  source: "frontend";
  userId?: string | null;
  anonymousId?: string | null;
  sessionId?: string | null;
  context?: Record<string, unknown> | null;
  payload?: Record<string, unknown> | null;
}

const modulePrefix = "/tracking";

export const trackEvent = async (
  cmd: TrackEventCommand
): Promise<ApiResult<string>> => {
  return await post<string>(`${modulePrefix}/track`, {
    EventType: cmd.eventType,
    Source: cmd.source,
    UserId: cmd.userId ?? undefined,
    AnonymousId: cmd.anonymousId ?? undefined,
    SessionId: cmd.sessionId ?? undefined,
    Context: cmd.context ?? undefined,
    Payload: cmd.payload ?? undefined,
  });
};

export const linkIdentity = async (
  anonymousId: string,
  userId: string
): Promise<ApiResult<void>> => {
  return await post<void>(`${modulePrefix}/link-identity`, {
    AnonymousId: anonymousId,
    UserId: userId,
  });
};
