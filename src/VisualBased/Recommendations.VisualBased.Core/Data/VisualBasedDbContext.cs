using Microsoft.EntityFrameworkCore;
using Recommendations.VisualBased.Core.Types;

namespace Recommendations.VisualBased.Core.Data;

public class VisualBasedDbContext : DbContext
{
    public VisualBasedDbContext(DbContextOptions<VisualBasedDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<ItemVisual> ItemVisuals => Set<ItemVisual>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Visual");
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.Entity<ItemVisual>(e =>
        {
            e.ToTable("item_embeddings_visual");
            e.HasKey(x => x.ItemId);
            e.Property(x => x.Emb).HasColumnType("vector(512)");
            e.HasIndex(x => x.GeneratedAt);
        });
    }
}