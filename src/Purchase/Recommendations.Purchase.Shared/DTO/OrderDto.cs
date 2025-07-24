namespace Recommendations.Purchase.Shared.DTO;

public record OrderDto(
    Guid IdOrder,
    Guid CustomerId,
    int Status,
    DateTime CreatedAt,
    DateTime? PaidAt,
    IReadOnlyCollection<OrderItemDto> Items,
    IEnumerable<PaymentDto>? Payments,
    AddressDto? Address);