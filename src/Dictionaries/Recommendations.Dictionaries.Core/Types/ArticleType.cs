namespace Recommendations.Dictionaries.Core.Types;

public sealed class ArticleType
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid SubCategoryId { get; private set; }
    public SubCategory SubCategory { get; private set; } = null!;

    private ArticleType(
        Guid id,
        string name,
        Guid subCategoryId)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Id = id;
        Name = name;
        SubCategoryId = subCategoryId;
    }

    public static ArticleType Create(
        string name,
        Guid subCategoryId)
    {
        return new ArticleType(
            Guid.NewGuid(),
            name,
            subCategoryId);
    }
} 