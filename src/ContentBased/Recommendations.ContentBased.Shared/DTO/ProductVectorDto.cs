using Pgvector;
using Recommendations.ContentBased.Shared.Types;
using Recommendations.Dictionaries.Shared.DTO;

namespace Recommendations.ContentBased.Shared.DTO;

public record ProductEmbeddingDto(
    Guid ProductId,
    VectorType Variant,
    Vector Embedding,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record CreateProductEmbeddingDto(
    Guid ProductId,
    VectorType Variant,
    Vector Embedding);

public record UpdateProductEmbeddingDto(
    Vector Embedding);

public record SimilarProductDto(
    Guid ProductId,
    float SimilarityScore);
