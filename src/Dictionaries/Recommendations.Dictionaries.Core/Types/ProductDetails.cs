namespace Recommendations.Dictionaries.Core.Types;

public sealed class ProductDetails
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public string Gender { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public string Usage { get; set; } = string.Empty;
    public string? Year { get; set; }
    
    public string? Description { get; set; }
    public string? SleeveLength { get; set; }
    public string? Fit { get; set; }
    public string? Fabric { get; set; }
    public string? Collar { get; set; }
    public string? BodyOrGarmentSize { get; set; }
    public string? Pattern { get; set; }
    public string? AgeGroup { get; set; }

    public ProductDetails()
    {
    }
    
    private ProductDetails(
        Guid id,
        Guid productId,
        string gender,
        string season,
        string usage,
        string? year,
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
        Year = year;
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
        string? year,
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
            year,
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