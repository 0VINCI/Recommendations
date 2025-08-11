namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record ProductDto(
    Guid Id,
    string ProductDisplayName,
    string BrandName,
    Guid SubCategoryId,
    string SubCategoryName,
    Guid ArticleTypeId,
    string ArticleTypeName,
    Guid BaseColourId,
    string BaseColourName,
    decimal Price,
    decimal? OriginalPrice,
    decimal Rating,
    int Reviews,
    bool IsBestseller,
    bool IsNew,
    ProductDetailsDto? Details,
    IReadOnlyCollection<ProductImageDto> Images); 