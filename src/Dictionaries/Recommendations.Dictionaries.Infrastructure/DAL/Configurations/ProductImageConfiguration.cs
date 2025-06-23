using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(pi => pi.Id);
        
        builder.Property(pi => pi.ImageUrl)
            .IsRequired()
            .HasMaxLength(1000);
            
        builder.Property(pi => pi.ImageType)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(pi => pi.Resolution)
            .HasMaxLength(20);
            
        builder.Property(pi => pi.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(pi => pi.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pi => pi.ImageType);
        builder.HasIndex(pi => pi.IsPrimary);
        builder.HasIndex(pi => pi.ProductId);
    }
} 