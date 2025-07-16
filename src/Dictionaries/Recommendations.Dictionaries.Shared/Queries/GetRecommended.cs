using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Shared.Queries;

public sealed record GetRecommended(
    int Page = 1,
    int PageSize = 20,
    string? SubCategoryId = null,
    string? MasterCategoryId = null,
    string? ArticleTypeId = null,
    string? BaseColourId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    bool? IsBestseller = null,
    bool? IsNew = null,
    string? SearchTerm = null
) : IQuery<FilteredProductDto>;
