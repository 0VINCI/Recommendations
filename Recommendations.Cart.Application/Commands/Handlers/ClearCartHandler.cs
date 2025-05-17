using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class ClearCartHandler(ICartRepository cartRepository, IUserContext userContext) : ICommandHandler<ClearCart>
{
    public async Task HandleAsync(ClearCart command, CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetLatestCartForUser(userContext.UserId, cancellationToken);
        if (cart is not null)
        {
            await cartRepository.Remove(cart.IdCart, cancellationToken);
        }
    }
}