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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Dictionary");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
} 