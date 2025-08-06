using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Configurations;

public class MasterCategoryConfiguration : IEntityTypeConfiguration<MasterCategory>
{
    public void Configure(EntityTypeBuilder<MasterCategory> builder)
    {
        builder.HasKey(mc => mc.Id);
        
        builder.Property(mc => mc.Name)
            .IsRequired()
            .HasMaxLength(100);
    
        builder.HasMany(mc => mc.SubCategories)
            .WithOne(sc => sc.MasterCategory)
            .HasForeignKey(sc => sc.MasterCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(mc => mc.Name)
            .IsUnique();
    }
}