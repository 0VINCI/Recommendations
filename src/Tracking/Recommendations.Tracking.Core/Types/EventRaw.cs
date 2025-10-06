using System.Text.Json;

namespace Recommendations.Tracking.Core.Types;

public sealed class EventRaw
{
    public Guid Id { get; set; }                         // event_id
    public DateTimeOffset Ts { get; set; }               // UTC
    public string Type { get; set; } = default!;
    public EventSource Source { get; set; }              // frontend/backend

    public string? UserId { get; set; }                  // jeśli zalogowany
    public Guid? AnonymousId { get; set; }               // jeśli gość
    public Guid? SessionId { get; set; }                 // z frontu

    public JsonDocument Context { get; set; } = default!; // jsonb
    public JsonDocument Payload { get; set; } = default!; // jsonb
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;

    // Wygodne kolumny generowane do indeksowania:
    public string? ItemId { get; private set; }          // generated from payload->>'item_id'
    public string? OrderId { get; private set; }         // generated from payload->>'order_id'
}
