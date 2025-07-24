using Recommendations.Purchase.Shared.DTO;
using Recommendations.Shared.Abstractions.Commands.CommandWithResult;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record CreateOrder(    
    Guid CustomerId,
    Guid ShippingAddressId,
    int Status,
    DateTime CreatedAt,
    DateTime? PaidAt,
    IReadOnlyCollection<OrderItemDto> Items,
    IEnumerable<PaymentDto>? Payments) : ICommandWithResult<Guid>;