using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Configurations;

public class ProductDetailsConfiguration : IEntityTypeConfiguration<ProductDetails>
{
    public void Configure(EntityTypeBuilder<ProductDetails> builder)
    {
        builder.HasKey(pd => pd.Id);
        
        builder.Property(pd => pd.Gender)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(pd => pd.Season)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(pd => pd.Year)
            .IsRequired();
            
        builder.Property(pd => pd.Usage)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pd => pd.Description);
            
        builder.Property(pd => pd.StyleNote)
            .HasMaxLength(5000);
            
        builder.Property(pd => pd.MaterialsCare)
            .HasMaxLength(2000);
            
        builder.Property(pd => pd.Fit)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.Fabric)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.ArticleNumber)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.Vat)
            .HasColumnType("decimal(5,2)");
            
        builder.Property(pd => pd.AgeGroup)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.FashionType)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.LandingPageUrl)
            .HasMaxLength(500);
            
        builder.Property(pd => pd.VariantName)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.MyntraRating)
            .HasColumnType("decimal(3,1)");
            
        builder.Property(pd => pd.CatalogAddDate);
            
        builder.Property(pd => pd.Colour1)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.Colour2)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.VisualTag)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.StyleType)
            .HasMaxLength(20);
            
        builder.Property(pd => pd.ProductTypeId);
            
        builder.Property(pd => pd.DisplayCategories)
            .HasMaxLength(200);
            
        builder.Property(pd => pd.Weight)
            .HasMaxLength(20);
            
        builder.Property(pd => pd.NavigationId);

        builder.HasOne(pd => pd.Product)
            .WithOne(p => p.Details)
            .HasForeignKey<ProductDetails>(pd => pd.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pd => pd.Gender);
        builder.HasIndex(pd => pd.Season);
        builder.HasIndex(pd => pd.Year);
        builder.HasIndex(pd => pd.Usage);
    }
} 