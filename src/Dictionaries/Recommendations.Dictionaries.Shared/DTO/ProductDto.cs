namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    decimal? OriginalPrice,
    string Image,
    string Category,
    string Description,
    IReadOnlyCollection<string>? Sizes,
    IReadOnlyCollection<string>? Colors,
    decimal Rating,
    int Reviews,
    bool IsBestseller,
    bool IsNew); 