namespace Recommendations.Dictionaries.Core.Types;

public sealed class ArticleType
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid SubCategoryId { get; private set; }
    public SubCategory SubCategory { get; private set; } = null!;
    public bool Active { get; private set; }
    public bool SocialSharingEnabled { get; private set; }
    public bool IsReturnable { get; private set; }
    public bool IsExchangeable { get; private set; }
    public bool PickupEnabled { get; private set; }
    public bool IsTryAndBuyEnabled { get; private set; }
    public bool IsMyntsEnabled { get; private set; }
    
    public ArticleType(
        Guid id,
        string name,
        Guid subCategoryId,
        bool active = true,
        bool socialSharingEnabled = true,
        bool isReturnable = true,
        bool isExchangeable = true,
        bool pickupEnabled = true,
        bool isTryAndBuyEnabled = true,
        bool isMyntsEnabled = true)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Id = id;
        Name = name;
        SubCategoryId = subCategoryId;
        Active = active;
        SocialSharingEnabled = socialSharingEnabled;
        IsReturnable = isReturnable;
        IsExchangeable = isExchangeable;
        PickupEnabled = pickupEnabled;
        IsTryAndBuyEnabled = isTryAndBuyEnabled;
        IsMyntsEnabled = isMyntsEnabled;
    }

    public static ArticleType Create(
        string name,
        Guid subCategoryId,
        bool active = true,
        bool socialSharingEnabled = true,
        bool isReturnable = true,
        bool isExchangeable = true,
        bool pickupEnabled = true,
        bool isTryAndBuyEnabled = true,
        bool isMyntsEnabled = true)
    {
        return new ArticleType(
            Guid.NewGuid(),
            name,
            subCategoryId,
            active,
            socialSharingEnabled,
            isReturnable,
            isExchangeable,
            pickupEnabled,
            isTryAndBuyEnabled,
            isMyntsEnabled);
    }
} 