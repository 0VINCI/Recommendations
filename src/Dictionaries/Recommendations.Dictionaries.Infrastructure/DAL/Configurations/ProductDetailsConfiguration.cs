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
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(pd => pd.Usage)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pd => pd.Description);
        
        builder.Property(pd => pd.SleeveLength)
            .HasMaxLength(100);
        
        builder.Property(pd => pd.Fit)
            .HasMaxLength(100);
            
        builder.Property(pd => pd.Fabric)
            .HasMaxLength(100);
        
        builder.Property(pd => pd.Collar)
            .HasMaxLength(100);

        builder.Property(pd => pd.BodyOrGarmentSize)
            .HasMaxLength(100);

        builder.Property(pd => pd.Pattern)
            .HasMaxLength(100);

        builder.Property(pd => pd.AgeGroup)
            .HasMaxLength(100);

        builder.HasOne(pd => pd.Product)
            .WithOne(p => p.Details)
            .HasForeignKey<ProductDetails>(pd => pd.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pd => pd.Gender);
        builder.HasIndex(pd => pd.Season);
        builder.HasIndex(pd => pd.Usage);
    }
} 