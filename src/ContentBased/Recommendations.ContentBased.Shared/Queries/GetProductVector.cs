using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Types;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.ContentBased.Shared.Queries;

public record GetProductEmbedding(Guid ProductId, VectorType Variant) : IQuery<ProductEmbeddingDto?>;
public record GetProductEmbeddings(Guid ProductId) : IQuery<IEnumerable<ProductEmbeddingDto>>;
