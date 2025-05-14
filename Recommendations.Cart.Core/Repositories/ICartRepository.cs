using Recommendations.Cart.Core.Types;

namespace Recommendations.Cart.Core.Repositories;

public interface ICartRepository
{
    public Task<ShoppingCart?> GetShoppingCart(Guid cartId, CancellationToken cancellationToken = default);
    Task<ShoppingCart?> GetLatestCartForUser(Guid userId, CancellationToken cancellationToken = default);
    Task<ShoppingCart> GetOrCreateCartForUser(Guid userId, CancellationToken cancellationToken = default);
    public Task Save(ShoppingCart cart, CancellationToken cancellationToken = default);
    public Task Remove(Guid cartId, CancellationToken cancellationToken = default);
}