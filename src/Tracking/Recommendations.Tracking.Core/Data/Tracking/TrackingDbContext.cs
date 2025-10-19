using Microsoft.EntityFrameworkCore;
using Recommendations.Tracking.Core.Types;

namespace Recommendations.Tracking.Core.Data.Tracking;

public sealed class TrackingDbContext : DbContext
{
    public DbSet<EventRaw> EventsRaw => Set<EventRaw>();
    public DbSet<IdentityLink> IdentityLinks => Set<IdentityLink>();
    public DbSet<EventRejected> EventsRejected => Set<EventRejected>();

    public TrackingDbContext(DbContextOptions<TrackingDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("Tracking");

        b.HasPostgresEnum<EventSource>("rec", "event_source");

        b.Entity<EventRaw>(e =>
        {
            e.ToTable("events_raw");
            e.HasKey(x => x.Id);
            e.Property(x => x.Source).HasColumnType("rec.event_source");
            e.Property(x => x.Context).HasColumnType("jsonb");
            e.Property(x => x.Payload).HasColumnType("jsonb");

            e.Property(x => x.ItemId)
                .HasComputedColumnSql("(\"Payload\"->>'item_id')", stored: true)
                .ValueGeneratedOnAddOrUpdate();
            e.Property(x => x.OrderId)
                .HasComputedColumnSql("(\"Payload\"->>'order_id')", stored: true)
                .ValueGeneratedOnAddOrUpdate();

            e.HasIndex(x => new { x.Type, x.Ts });
            e.HasIndex(x => x.UserId);
            e.HasIndex(x => x.AnonymousId);
            e.HasIndex(x => x.ItemId);
            e.HasIndex(x => x.OrderId);
        });

        b.Entity<IdentityLink>(e =>
        {
            e.ToTable("identity_links");
            e.HasKey(x => new { x.AnonymousId, x.UserId });
        });

        b.Entity<EventRejected>(e =>
        {
            e.ToTable("events_rejected");
            e.HasKey(x => x.Id);
            e.Property(x => x.Raw).HasColumnType("jsonb");
        });
    }
}
