using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Events;
using Recommendations.Shared.Abstractions.UserContext;
using Recommendations.Tracking.Shared.Events;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class UpdateCartItemQuantityHandler(
    ICartRepository cartRepository, 
    IUserContext userContext,
    IEventDispatcher eventDispatcher) : ICommandHandler<UpdateCartItemQuantity>
{
    public async Task HandleAsync(UpdateCartItemQuantity command, CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetOrCreateCartForUser(userContext.UserId, cancellationToken);
        
        var itemToUpdate = cart.Items.FirstOrDefault(i => i.ProductId == command.ProductId);
        if (itemToUpdate is null)
        {
            return;
        }

        var oldQuantity = itemToUpdate.Quantity;
        cart.UpdateItemQuantity(command.ProductId, command.Quantity);
        await cartRepository.Save(cart, cancellationToken);

        var cartItemQuantityChangedEvent = new CartItemQuantityChanged(
            userContext.UserId,
            itemToUpdate.ProductId,
            itemToUpdate.Name,
            itemToUpdate.UnitPrice,
            oldQuantity,
            command.Quantity,
            DateTime.UtcNow
        );
        
        await eventDispatcher.PublishAsync(cartItemQuantityChangedEvent);
    }
}