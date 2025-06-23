using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Configurations;

public class ArticleTypeConfiguration : IEntityTypeConfiguration<ArticleType>
{
    public void Configure(EntityTypeBuilder<ArticleType> builder)
    {
        builder.HasKey(at => at.Id);
        
        builder.Property(at => at.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(at => at.Active)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(at => at.SocialSharingEnabled)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(at => at.IsReturnable)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(at => at.IsExchangeable)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(at => at.PickupEnabled)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(at => at.IsTryAndBuyEnabled)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(at => at.IsMyntsEnabled)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(at => at.SubCategory)
            .WithMany(sc => sc.ArticleTypes)
            .HasForeignKey(at => at.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(at => at.Name).IsUnique();
        builder.HasIndex(at => at.Active);
    }
} 