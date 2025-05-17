using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class RemoveItemFromCartHandler(ICartRepository cartRepository, IUserContext userContext) : ICommandHandler<RemoveItemFromCart>
{
    public async Task HandleAsync(RemoveItemFromCart command, CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetLatestCartForUser(userContext.UserId, cancellationToken);
        
        if (cart is null)
        {
            return;
        }
        
        cart.RemoveItem(command.ProductId);
        
        await (cart.Items.Count != 0
            ? cartRepository.Save(cart, cancellationToken)
            : cartRepository.Remove(cart.IdCart, cancellationToken));
    }
}