using AutoMapper;
using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Shared.DTO;
using Recommendations.Cart.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Cart.Application.Queries.Handlers;

public sealed class GetCartHandler
    (ICartRepository cartRepository, IMapper mapper) : IQueryHandler<GetCart, ShoppingCartDto?>
{
    public async Task<ShoppingCartDto?> HandleAsync(GetCart query,
        CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetShoppingCart(query.CartId, cancellationToken);
            
        return cart is null ? null : mapper.Map<ShoppingCartDto>(cart);
    }
}