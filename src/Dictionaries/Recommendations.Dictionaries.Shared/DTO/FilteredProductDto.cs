namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record FilteredProductDto(
    IReadOnlyCollection<ProductDto> Products,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
