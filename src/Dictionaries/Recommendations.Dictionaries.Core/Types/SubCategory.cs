namespace Recommendations.Dictionaries.Core.Types;

public sealed class SubCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid MasterCategoryId { get; private set; }
    public MasterCategory MasterCategory { get; private set; } = null!;
    public bool Active { get; private set; }
    public bool SocialSharingEnabled { get; private set; }
    public bool IsReturnable { get; private set; }
    public bool IsExchangeable { get; private set; }
    public bool PickupEnabled { get; private set; }
    public bool IsTryAndBuyEnabled { get; private set; }
    
    public ICollection<ArticleType> ArticleTypes { get; private set; } = new List<ArticleType>();
    
    public SubCategory(
        Guid id,
        string name,
        Guid masterCategoryId,
        bool active = true,
        bool socialSharingEnabled = true,
        bool isReturnable = true,
        bool isExchangeable = true,
        bool pickupEnabled = true,
        bool isTryAndBuyEnabled = true)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Id = id;
        Name = name;
        MasterCategoryId = masterCategoryId;
        Active = active;
        SocialSharingEnabled = socialSharingEnabled;
        IsReturnable = isReturnable;
        IsExchangeable = isExchangeable;
        PickupEnabled = pickupEnabled;
        IsTryAndBuyEnabled = isTryAndBuyEnabled;
    }

    public static SubCategory Create(
        string name,
        Guid masterCategoryId,
        bool active = true,
        bool socialSharingEnabled = true,
        bool isReturnable = true,
        bool isExchangeable = true,
        bool pickupEnabled = true,
        bool isTryAndBuyEnabled = true)
    {
        return new SubCategory(
            Guid.NewGuid(),
            name,
            masterCategoryId,
            active,
            socialSharingEnabled,
            isReturnable,
            isExchangeable,
            pickupEnabled,
            isTryAndBuyEnabled);
    }
} 