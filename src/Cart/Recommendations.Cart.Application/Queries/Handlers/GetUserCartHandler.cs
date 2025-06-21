using AutoMapper;
using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Shared.DTO;
using Recommendations.Cart.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Cart.Application.Queries.Handlers;

public sealed class GetUserCartHandler
    (ICartRepository cartRepository, IMapper mapper, IUserContext userContext) : IQueryHandler<GetUserCart, ShoppingCartDto?>
{
    public async Task<ShoppingCartDto?> HandleAsync(GetUserCart query,
        CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetLatestCartForUser(userContext.UserId, cancellationToken);
            
        return cart is null ? null : mapper.Map<ShoppingCartDto>(cart);
    }
} 