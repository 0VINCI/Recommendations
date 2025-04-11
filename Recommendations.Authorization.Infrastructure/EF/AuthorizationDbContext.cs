using Microsoft.EntityFrameworkCore;
using Recommendations.Authorization.Core.Types;

namespace Recommendations.Authorization.Infrastructure.EF;

public class AuthorizationDbContext : DbContext
{
    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Authorization");

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.IdUser);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Surname)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(u => u.Password)
                .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
