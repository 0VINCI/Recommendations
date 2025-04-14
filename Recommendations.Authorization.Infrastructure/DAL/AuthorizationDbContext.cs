using Microsoft.EntityFrameworkCore;
using Recommendations.Authorization.Core.Types;

namespace Recommendations.Authorization.Infrastructure.DAL;

public class AuthorizationDbContext : DbContext
{
    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Authorization");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}