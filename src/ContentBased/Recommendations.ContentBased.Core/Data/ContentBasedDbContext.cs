using Microsoft.EntityFrameworkCore;
using Recommendations.ContentBased.Core.Types;

namespace Recommendations.ContentBased.Core.Data;

public class ContentBasedDbContext : DbContext
{
    public ContentBasedDbContext(DbContextOptions<ContentBasedDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<ProductEmbedding> ProductEmbeddings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ContentBased");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
