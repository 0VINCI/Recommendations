using Recommendations.Shared.Abstractions.Events;

namespace Recommendations.Tracking.Shared.Events;

public sealed record OrderPaid(
    Guid UserId,
    Guid OrderId,
    decimal TotalAmount,
    string PaymentMethod,
    DateTime Timestamp
) : IEvent; 