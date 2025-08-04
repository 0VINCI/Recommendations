namespace Recommendations.Dictionaries.Core.Types;

public sealed class ProductDetails
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    
    public string Gender { get; private set; }
    public string Season { get; private set; }
    public string Usage { get; private set; }
    
    public string? Description { get; private set; }
    public string? SleeveLength { get; private set; }
    public string? Fit { get; private set; }
    public string? Fabric { get; private set; }
    public string? Collar { get; private set; }
    public string? BodyOrGarmentSize { get; private set; }
    public string? Pattern { get; private set; }
    public string? AgeGroup { get; private set; }

    private ProductDetails(
        Guid id,
        Guid productId,
        string gender,
        string season,
        string usage,
        string? description = null,
        string? sleeveLength = null,
        string? fit = null,
        string? fabric = null,
        string? collar = null,
        string? bodyOrGarmentSize = null,
        string? pattern = null,
        string? ageGroup = null)
    {
        if (string.IsNullOrWhiteSpace(gender)) 
            throw new ArgumentException("Gender cannot be empty", nameof(gender));
        if (string.IsNullOrWhiteSpace(season)) 
            throw new ArgumentException("Season cannot be empty", nameof(season));
        if (string.IsNullOrWhiteSpace(usage)) 
            throw new ArgumentException("Usage cannot be empty", nameof(usage));

        Id = id;
        ProductId = productId;
        Gender = gender;
        Season = season;
        Usage = usage;
        Description = description;
        SleeveLength = sleeveLength;
        Fit = fit;
        Fabric = fabric;
        Collar = collar;
        BodyOrGarmentSize = bodyOrGarmentSize;
        Pattern = pattern;
        AgeGroup = ageGroup;
    }

    public static ProductDetails Create(
        Guid productId,
        string gender,
        string season,
        string usage,
        string? description = null,
        string? sleeveLength = null,
        string? fit = null,
        string? fabric = null,
        string? collar = null,
        string? bodyOrGarmentSize = null,
        string? pattern = null,
        string? ageGroup = null)
    {
        return new ProductDetails(
            Guid.NewGuid(),
            productId,
            gender,
            season,
            usage,
            description,
            sleeveLength,
            fit,
            fabric,
            collar,
            bodyOrGarmentSize,
            pattern,
            ageGroup);
    }
} 