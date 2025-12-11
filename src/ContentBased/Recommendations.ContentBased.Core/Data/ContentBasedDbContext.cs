using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Recommendations.ContentBased.Core.Types;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.ContentBased.Core.Data;

public class ContentBasedDbContext : DbContext
{
    public ContentBasedDbContext(DbContextOptions<ContentBasedDbContext> dbContextOptions, IOptions<DbOptions> dbOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<ProductEmbedding> ProductEmbeddings { get; set; }
    public DbSet<ProductEmbeddingNew> ProductEmbeddingsNew { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Vectors");
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
