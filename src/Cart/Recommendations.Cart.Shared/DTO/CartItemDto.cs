namespace Recommendations.Cart.Shared.DTO;

public record CartItemDto(
    Guid   ProductId,
    string Name,
    int    Quantity,
    decimal UnitPrice,
    decimal Subtotal);