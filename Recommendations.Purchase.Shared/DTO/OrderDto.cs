namespace Recommendations.Purchase.Shared.DTO;

public record OrderDto(
    Guid IdOrder,
    Guid CustomerId,
    Guid ShippingAddressId,
    int Status,
    DateTime CreatedAt,
    DateTime? PaidAt,
    IReadOnlyCollection<OrderItemDto> Items
    );