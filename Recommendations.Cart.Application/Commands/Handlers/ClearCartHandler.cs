using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Cart.Application.Commands.Handlers;

internal sealed class ClearCartHandler() : ICommandHandler<ClearCart>
{
    public async Task HandleAsync(ClearCart command, CancellationToken cancellationToken = default)
    {
    }
}