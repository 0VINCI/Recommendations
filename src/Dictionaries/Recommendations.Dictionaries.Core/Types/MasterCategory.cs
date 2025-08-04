namespace Recommendations.Dictionaries.Core.Types;

public sealed class MasterCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public ICollection<SubCategory> SubCategories { get; private set; } = new List<SubCategory>();
    
    public MasterCategory(
        Guid id,
        string name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Id = id;
        Name = name;
    }

    public static MasterCategory Create(
        string name)
    {
        return new MasterCategory(
            Guid.NewGuid(),
            name);
    }
} 