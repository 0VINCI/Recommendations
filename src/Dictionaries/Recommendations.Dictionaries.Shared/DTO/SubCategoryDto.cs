namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record SubCategoryDto(
    Guid Id,
    string Name,
    Guid MasterCategoryId,
    string MasterCategoryName,
    bool Active,
    bool SocialSharingEnabled,
    bool IsReturnable,
    bool IsExchangeable,
    bool PickupEnabled,
    bool IsTryAndBuyEnabled); 