using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Configurations;

public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.HasKey(sc => sc.Id);
        
        builder.Property(sc => sc.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.HasOne(sc => sc.MasterCategory)
            .WithMany(mc => mc.SubCategories)
            .HasForeignKey(sc => sc.MasterCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(sc => sc.ArticleTypes)
            .WithOne(at => at.SubCategory)
            .HasForeignKey(at => at.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sc => sc.Name).IsUnique();
    }
} 