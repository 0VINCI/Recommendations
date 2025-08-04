namespace Recommendations.Dictionaries.Core.Types;

public sealed class SubCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid MasterCategoryId { get; private set; }
    public MasterCategory MasterCategory { get; private set; } = null!;
    
    public ICollection<ArticleType> ArticleTypes { get; private set; } = new List<ArticleType>();
    
    public SubCategory(
        Guid id,
        string name,
        Guid masterCategoryId)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Id = id;
        Name = name;
        MasterCategoryId = masterCategoryId;
    }

    public static SubCategory Create(
        string name,
        Guid masterCategoryId)
    {
        return new SubCategory(
            Guid.NewGuid(),
            name,
            masterCategoryId);
    }
} 