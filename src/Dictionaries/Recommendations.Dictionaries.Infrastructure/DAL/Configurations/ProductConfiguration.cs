using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.ProductDisplayName)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(p => p.BrandName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(p => p.OriginalPrice)
            .HasColumnType("decimal(18,2)");
            
        builder.Property(p => p.Rating)
            .HasColumnType("decimal(3,1)")
            .IsRequired();
            
        builder.Property(p => p.Reviews)
            .IsRequired();
            
        builder.Property(p => p.IsBestseller)
            .IsRequired();
            
        builder.Property(p => p.IsNew)
            .IsRequired();
            
        builder.Property(p => p.IsTrending)
            .IsRequired();
            
        builder.Property(p => p.IsOnSale)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(p => p.ProfitBoost)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(p => p.SubCategory)
            .WithMany()
            .HasForeignKey(p => p.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(p => p.ArticleType)
            .WithMany()
            .HasForeignKey(p => p.ArticleTypeId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(p => p.BaseColour)
            .WithMany(bc => bc.Products)
            .HasForeignKey(p => p.BaseColourId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(p => p.Details)
            .WithOne(d => d.Product)
            .HasForeignKey<ProductDetails>(d => d.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(p => p.Images)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.BrandName);
        builder.HasIndex(p => p.IsBestseller);
        builder.HasIndex(p => p.IsNew);
        builder.HasIndex(p => p.IsTrending);
        builder.HasIndex(p => p.IsOnSale);
        builder.HasIndex(p => p.ProfitBoost);
        builder.HasIndex(p => p.Rating);
        builder.HasIndex(p => p.Price);
    }
} 