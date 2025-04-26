using Microsoft.EntityFrameworkCore;
using Recommendations.Cart.Core.Types;

namespace Recommendations.Cart.Infrastructure.DAL
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<ShoppingCart> Cart { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Cart");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}