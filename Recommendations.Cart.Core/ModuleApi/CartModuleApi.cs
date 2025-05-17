using Recommendations.Cart.Shared;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Cart.Shared.DTO;
using Recommendations.Cart.Shared.Queries;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Cart.Core.ModuleApi;
internal class CartModuleApi(ICommandDispatcher commands,
        IQueryDispatcher queries)
    : ICartModuleApi
{
    public Task AddItem(AddItemToCart cmd) => commands.SendAsync(cmd);
    
    public Task RemoveItem(RemoveItemFromCart cmd) => commands.SendAsync(cmd);

    public Task UpdateQuantity(UpdateCartItemQuantity cmd) => commands.SendAsync(cmd);

    public Task ClearCart(ClearCart cmd) => commands.SendAsync(cmd);
    
    public Task<ShoppingCartDto?> GetCart(GetCart query) => queries.QueryAsync(new GetCart(query.CartId));
}