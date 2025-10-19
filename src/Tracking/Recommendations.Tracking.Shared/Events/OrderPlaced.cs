using Recommendations.Shared.Abstractions.Events;

namespace Recommendations.Tracking.Shared.Events;

public sealed record OrderPlaced(
    Guid UserId,
    Guid OrderId,
    decimal TotalAmount,
    int ItemsCount,
    Items[] Items,
    DateTime Timestamp
) : IEvent;

public sealed record Items(
    Guid ProductId,
    decimal ProductPrice,
    int Quantity
);
