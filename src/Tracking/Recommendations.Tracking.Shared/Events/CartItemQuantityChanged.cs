using Recommendations.Shared.Abstractions.Events;

namespace Recommendations.Tracking.Shared.Events;

public sealed record CartItemQuantityChanged(
    Guid UserId,
    Guid ProductId,
    string ProductName,
    decimal ProductPrice,
    int OldQuantity,
    int NewQuantity,
    DateTime Timestamp
) : IEvent;
