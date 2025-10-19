using Recommendations.Shared.Abstractions.Events;

namespace Recommendations.Tracking.Shared.Events;

public sealed record CartCleared(
    Guid UserId,
    DateTime Timestamp
) : IEvent;
