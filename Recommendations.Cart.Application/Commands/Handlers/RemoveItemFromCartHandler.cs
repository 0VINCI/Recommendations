using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class RemoveItemFromCartHandler() : ICommandHandler<RemoveItemFromCart>
{
    public async Task HandleAsync(RemoveItemFromCart command, CancellationToken cancellationToken = default)
    {
    }
}