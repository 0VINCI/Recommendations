namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record MasterCategoryDto(
    Guid Id,
    string Name,
    bool Active,
    bool SocialSharingEnabled,
    bool IsReturnable,
    bool IsExchangeable,
    bool PickupEnabled,
    bool IsTryAndBuyEnabled); 