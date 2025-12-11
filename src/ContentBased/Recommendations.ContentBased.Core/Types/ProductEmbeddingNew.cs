using Pgvector;
using Recommendations.ContentBased.Shared.Types;

namespace Recommendations.ContentBased.Core.Types;

public sealed class ProductEmbedding
{
    public Guid ProductId { get; private set; }
    public VectorType Variant { get; private set; }
    public Vector Embedding { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private ProductEmbedding() { }

    public static ProductEmbedding Create(Guid productId, VectorType variant, Vector embedding)
    {
        Ensure768(embedding);
        return new ProductEmbedding
        {
            ProductId = productId,
            Variant   = variant,
            Embedding = embedding,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(Vector embedding)
    {
        Ensure768(embedding);
        Embedding = embedding;
        UpdatedAt = DateTime.UtcNow;
    }

    static void Ensure768(Vector v)
    {
        if (v is null)
            throw new ArgumentException("Embedding cannot be null", nameof(v));
        
    }
}
