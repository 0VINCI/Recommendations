using Microsoft.EntityFrameworkCore;
using Recommendations.Tracking.Core.Types;

namespace Recommendations.Tracking.Core.Data.Signals;

public sealed class SignalsDbContext : DbContext
{
    public DbSet<UserItemInteraction> UserItemInteractions => Set<UserItemInteraction>();
    public DbSet<UserEmbeddingOnline> UserEmbeddingsOnline => Set<UserEmbeddingOnline>();
    public DbSet<UserEmbeddingCf> UserEmbeddingsCf => Set<UserEmbeddingCf>();
    public DbSet<ItemEmbeddingCf> ItemEmbeddingsCf => Set<ItemEmbeddingCf>();

    public SignalsDbContext(DbContextOptions<SignalsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("Tracking");
        b.HasPostgresExtension("vector");

        b.Entity<UserItemInteraction>(e =>
        {
            e.ToTable("user_item_interactions");
            e.HasKey(x => new { x.UserKey, x.ItemId });
            e.HasIndex(x => x.LastTs).HasDatabaseName("ix_uii_last_ts");
        });

        b.Entity<UserEmbeddingOnline>(e =>
        {
            e.ToTable("user_embeddings_online");
            e.HasKey(x => x.UserKey);
            e.Property(x => x.Emb).HasColumnType("vector(768)");
            e.HasIndex(x => x.UpdatedAt);
        });

        b.Entity<UserEmbeddingCf>(e =>
        {
            e.ToTable("user_embeddings_cf");
            e.HasKey(x => x.UserKey);
            e.Property(x => x.Emb).HasColumnType("vector(128)");
            e.HasIndex(x => x.TrainedAt);
        });

        b.Entity<ItemEmbeddingCf>(e =>
        {
            e.ToTable("item_embeddings_cf");
            e.HasKey(x => x.ItemId);
            e.Property(x => x.Emb).HasColumnType("vector(128)");
            e.HasIndex(x => x.TrainedAt);
        });
    }
}
