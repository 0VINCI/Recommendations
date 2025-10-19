using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Events;
using Recommendations.Shared.Abstractions.UserContext;
using Recommendations.Tracking.Shared.Events;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class RemoveItemFromCartHandler(
    ICartRepository cartRepository, 
    IUserContext userContext,
    IEventDispatcher eventDispatcher) : ICommandHandler<RemoveItemFromCart>
{
    public async Task HandleAsync(RemoveItemFromCart command, CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetLatestCartForUser(userContext.UserId, cancellationToken);
        
        if (cart is null)
        {
            return;
        }

        var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == command.ProductId);
        if (itemToRemove is null)
        {
            return;
        }
        
        cart.RemoveItem(command.ProductId);
        
        await (cart.Items.Count != 0
            ? cartRepository.Save(cart, cancellationToken)
            : cartRepository.Remove(cart.IdCart, cancellationToken));

        var cartItemRemovedEvent = new CartItemRemoved(
            userContext.UserId,
            itemToRemove.ProductId,
            itemToRemove.Name,
            itemToRemove.UnitPrice,
            itemToRemove.Quantity,
            DateTime.UtcNow
        );
        
        await eventDispatcher.PublishAsync(cartItemRemovedEvent);
    }
}