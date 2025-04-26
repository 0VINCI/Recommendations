using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class UpdateCartItemQuantityHandler() : ICommandHandler<UpdateCartItemQuantity>
{
    public async Task HandleAsync(UpdateCartItemQuantity command, CancellationToken cancellationToken = default)
    {
    }
}