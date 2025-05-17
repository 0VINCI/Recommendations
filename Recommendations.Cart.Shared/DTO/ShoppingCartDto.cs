namespace Recommendations.Cart.Shared.DTO;

public record ShoppingCartDto(
    Guid IdCart,
    DateTime CreatedAt,
    decimal Total,
    IReadOnlyCollection<CartItemDto> Items);