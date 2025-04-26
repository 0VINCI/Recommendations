using Recommendations.Cart.Core.Types;

namespace Recommendations.Cart.Infrastructure.DAL.Repositories;

public interface ICartRepository
{
    public Task<ShoppingCart?> GetShoppingCart(Guid cartId, CancellationToken cancellationToken = default);
    public Task Save(ShoppingCart cart, CancellationToken cancellationToken = default);
    public Task Remove(Guid cartId, CancellationToken cancellationToken = default);
}