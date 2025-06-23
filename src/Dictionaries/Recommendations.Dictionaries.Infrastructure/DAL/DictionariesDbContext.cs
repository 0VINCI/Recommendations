using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL;

public class DictionariesDbContext : DbContext
{
    public DictionariesDbContext(DbContextOptions<DictionariesDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductDetails> ProductDetails { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    
    public DbSet<MasterCategory> MasterCategories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<ArticleType> ArticleTypes { get; set; }
    public DbSet<BaseColour> BaseColours { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Dictionary");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
} 