using Recommendations.Cart.Shared.DTO;
using Recommendations.Cart.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Cart.Application.Queries.Handlers;

public sealed class GetCartHandler() : IQueryHandler<GetCart, IReadOnlyCollection<CartItemDto>>
{
    public Task<IReadOnlyCollection<CartItemDto>> HandleAsync(GetCart query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}