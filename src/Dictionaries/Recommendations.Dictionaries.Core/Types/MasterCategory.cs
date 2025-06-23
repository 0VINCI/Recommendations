namespace Recommendations.Dictionaries.Core.Types;

public sealed class MasterCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool Active { get; private set; }
    public bool SocialSharingEnabled { get; private set; }
    public bool IsReturnable { get; private set; }
    public bool IsExchangeable { get; private set; }
    public bool PickupEnabled { get; private set; }
    public bool IsTryAndBuyEnabled { get; private set; }
    
    public ICollection<SubCategory> SubCategories { get; private set; } = new List<SubCategory>();
    
    public MasterCategory(
        Guid id,
        string name,
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
        Active = active;
        SocialSharingEnabled = socialSharingEnabled;
        IsReturnable = isReturnable;
        IsExchangeable = isExchangeable;
        PickupEnabled = pickupEnabled;
        IsTryAndBuyEnabled = isTryAndBuyEnabled;
    }

    public static MasterCategory Create(
        string name,
        bool active = true,
        bool socialSharingEnabled = true,
        bool isReturnable = true,
        bool isExchangeable = true,
        bool pickupEnabled = true,
        bool isTryAndBuyEnabled = true)
    {
        return new MasterCategory(
            Guid.NewGuid(),
            name,
            active,
            socialSharingEnabled,
            isReturnable,
            isExchangeable,
            pickupEnabled,
            isTryAndBuyEnabled);
    }
} 