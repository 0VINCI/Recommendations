using Pgvector;
using Recommendations.ContentBased.Shared.Types;

namespace Recommendations.ContentBased.Core.Types;

public sealed class ProductEmbeddingNew
{
    public Guid ProductId { get; private set; }
    public VectorType Variant { get; private set; }
    public Vector Embedding { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private ProductEmbeddingNew() { }

    public static ProductEmbeddingNew Create(Guid productId, VectorType variant, Vector embedding)
    {
        Ensure2560(embedding);
        return new ProductEmbeddingNew
        {
            ProductId = productId,
            Variant   = variant,
            Embedding = embedding,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(Vector embedding)
    {
        Ensure2560(embedding);
        Embedding = embedding;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Ensure2560(Vector v)
    {
        if (v is null)
            throw new ArgumentException("Embedding cannot be null", nameof(v));
        
    }
}
