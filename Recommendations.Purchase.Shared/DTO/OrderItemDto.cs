namespace Recommendations.Purchase.Shared.DTO;

public record OrderItemDto(
    Guid IdOrderItem,
    Guid ProductId,
    string ProductName,
    decimal ProductPrice,
    int Quantity
    );