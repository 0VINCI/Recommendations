using Microsoft.EntityFrameworkCore;
using Recommendations.Purchase.Core.Data.Models;

namespace Recommendations.Purchase.Core.Data;
public class PurchaseDbContext : DbContext
{
    public PurchaseDbContext(DbContextOptions<PurchaseDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<OrderDbModel> Orders { get; set; }
    public DbSet<CustomerDbModel> Customers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Purchase");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}