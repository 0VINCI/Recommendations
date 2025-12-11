using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recommendations.ContentBased.Core.Types;

namespace Recommendations.ContentBased.Core.Data.EF.Configurations;

internal sealed class ProductEmbeddingNewConfiguration : IEntityTypeConfiguration<ProductEmbeddingNew>
{
    public void Configure(EntityTypeBuilder<ProductEmbeddingNew> builder)
    {
        builder.ToTable("ProductEmbeddingsNew", "Vectors");
        builder.HasKey(x => new { x.ProductId, x.Variant });

        builder.Property(x => x.Variant).HasConversion<string>().HasMaxLength(40);

        builder.Property(x => x.Embedding)
             .HasColumnType("vector(2560)")
             .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.Variant);
        builder.HasIndex(x => x.CreatedAt);
    }
}
