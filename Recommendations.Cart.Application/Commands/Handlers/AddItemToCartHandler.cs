using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class AddItemToCartHandler() : ICommandHandler<AddItemToCart>
{
    public async Task HandleAsync(AddItemToCart command, CancellationToken cancellationToken = default)
    {
    }
}