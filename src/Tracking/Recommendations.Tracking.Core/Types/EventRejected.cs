using System.Text.Json;

namespace Recommendations.Tracking.Core.Types;

public sealed class EventRejected
{
    public Guid Id { get; set; }
    public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
    public string Reason { get; set; } = default!;
    public JsonDocument Raw { get; set; } = default!;
}