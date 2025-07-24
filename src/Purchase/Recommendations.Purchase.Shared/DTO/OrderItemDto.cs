namespace Recommendations.Purchase.Shared.DTO;

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal ProductPrice,
    int Quantity
    );