using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Shared.Queries;

public sealed record GetTrendingProducts(
    int Page = 1,
    int PageSize = 20
) : IQuery<FilteredProductDto>;

