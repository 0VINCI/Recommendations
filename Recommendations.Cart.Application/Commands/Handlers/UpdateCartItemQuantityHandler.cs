using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class UpdateCartItemQuantityHandler(ICartRepository cartRepository, IUserContext userContext) : ICommandHandler<UpdateCartItemQuantity>
{
    public async Task HandleAsync(UpdateCartItemQuantity command, CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetOrCreateCartForUser(userContext.UserId, cancellationToken);
        cart.UpdateItemQuantity(command.ProductId, command.Quantity);
        await cartRepository.Save(cart, cancellationToken);
    }
}