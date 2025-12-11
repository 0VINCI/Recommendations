using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Types;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.ContentBased.Shared.Queries;

public record GetSimilarProducts(
    Guid ProductId,
    VectorType Variant,
    int TopCount,
    bool UseNew = true) : IQuery<IEnumerable<SimilarProductDto>>;
