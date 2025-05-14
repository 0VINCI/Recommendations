using Microsoft.EntityFrameworkCore;
using Recommendations.Cart.Core.Repositories;
using Recommendations.Cart.Core.Types;

namespace Recommendations.Cart.Infrastructure.DAL.Repositories;

internal sealed class CartRepository(CartDbContext dbContext) : ICartRepository
{
    public async Task<ShoppingCart?> GetShoppingCart(Guid cartId, CancellationToken cancellationToken = default)
        => await dbContext.Cart
            .Include(c => c.Items)
            .SingleOrDefaultAsync(c => c.IdCart == cartId, cancellationToken);
    
    public async Task<ShoppingCart?> GetLatestCartForUser(Guid userId, CancellationToken cancellationToken = default)
        => await dbContext.Cart
            .Include(c => c.Items)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    
    public async Task<ShoppingCart> GetOrCreateCartForUser(Guid userId, CancellationToken cancellationToken = default)
    {
        var cart = await GetLatestCartForUser(userId, cancellationToken);
        if (cart is not null) return cart;

        cart = new ShoppingCart(userId);
        dbContext.Cart.Add(cart);
        await dbContext.SaveChangesAsync(cancellationToken);
        return cart;
    }

    public async Task Save(ShoppingCart cart, CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
    
    public async Task Remove(Guid cartId, CancellationToken cancellationToken = default)
    {
        var cart = await dbContext.Cart.SingleOrDefaultAsync(c => c.IdCart == cartId, cancellationToken);
        if (cart is null) return;
        dbContext.Cart.Remove(cart);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}