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
            
        builder.Property(mc => mc.Active)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(mc => mc.SocialSharingEnabled)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(mc => mc.IsReturnable)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(mc => mc.IsExchangeable)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(mc => mc.PickupEnabled)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(mc => mc.IsTryAndBuyEnabled)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasMany(mc => mc.SubCategories)
            .WithOne(sc => sc.MasterCategory)
            .HasForeignKey(sc => sc.MasterCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(mc => mc.Name)
            .IsUnique();
        builder.HasIndex(mc => mc.Active);
    }
}