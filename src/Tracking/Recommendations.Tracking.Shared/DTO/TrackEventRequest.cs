namespace Recommendations.Tracking.Shared.DTO;

public record TrackEventRequest(
    string EventType,
    string Source, // "Frontend" or "Backend"
    string? UserId = null,
    Guid? AnonymousId = null,
    Guid? SessionId = null,
    object? Context = null,
    object? Payload = null
);
