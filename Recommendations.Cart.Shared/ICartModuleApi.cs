using Recommendations.Cart.Shared.Commands;
using Recommendations.Cart.Shared.DTO;
using Recommendations.Cart.Shared.Queries;

namespace Recommendations.Cart.Shared;

public interface ICartModuleApi
{
    Task AddItem(AddItemToCart cmd);
    Task RemoveItem(RemoveItemFromCart cmd);
    Task UpdateQuantity(UpdateCartItemQuantity cmd);
    Task ClearCart(ClearCart cmd);
    Task<ShoppingCartDto?> GetCart(GetCart query);
}