using Recommendations.Shared.Abstractions.Events;

namespace Recommendations.Tracking.Shared.Events;

public sealed record CartItemRemoved(
    Guid UserId,
    Guid ProductId,
    string ProductName,
    decimal ProductPrice,
    int Quantity,
    DateTime Timestamp
) : IEvent;
