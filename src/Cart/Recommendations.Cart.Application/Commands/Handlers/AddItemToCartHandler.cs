using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class AddItemToCartHandler(ICartRepository cartRepository, IUserContext userContext) : ICommandHandler<AddItemToCart>
{
    public async Task HandleAsync(AddItemToCart command, CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetOrCreateCartForUser(userContext.UserId, cancellationToken);
        
        cart.AddItem(command.ProductId, command.Name, command.Price, command.Quantity);
        await cartRepository.Save(cart, cancellationToken);
    }
}