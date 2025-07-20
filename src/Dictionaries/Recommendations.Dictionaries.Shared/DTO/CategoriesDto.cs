namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record CategoriesDto(
    Guid Id,
    string Name,
    bool Active,
    bool SocialSharingEnabled,
    bool IsReturnable,
    bool IsExchangeable,
    bool PickupEnabled,
    bool IsTryAndBuyEnabled,
    IReadOnlyCollection<SubCategoryDto> SubCategories); 