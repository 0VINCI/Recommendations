using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Events;
using Recommendations.Shared.Abstractions.UserContext;
using Recommendations.Tracking.Shared.Events;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class AddItemToCartHandler(
    ICartRepository cartRepository, 
    IUserContext userContext,
    IEventDispatcher eventDispatcher) : ICommandHandler<AddItemToCart>
{
    public async Task HandleAsync(AddItemToCart command, CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetOrCreateCartForUser(userContext.UserId, cancellationToken);
        
        cart.AddItem(command.ProductId, command.Name, command.Price, command.Quantity);
        await cartRepository.Save(cart, cancellationToken);

        var cartItemAddedEvent = new CartItemAdded(
            userContext.UserId,
            command.ProductId,
            command.Name,
            command.Price,
            command.Quantity,
            DateTime.UtcNow
        );
        
        await eventDispatcher.PublishAsync(cartItemAddedEvent);
    }
}