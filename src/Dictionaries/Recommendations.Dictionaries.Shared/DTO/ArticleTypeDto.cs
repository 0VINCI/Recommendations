namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record ArticleTypeDto(
    Guid Id,
    string Name,
    Guid SubCategoryId,
    string SubCategoryName,
    bool Active,
    bool SocialSharingEnabled,
    bool IsReturnable,
    bool IsExchangeable,
    bool PickupEnabled,
    bool IsTryAndBuyEnabled,
    bool IsMyntsEnabled); 