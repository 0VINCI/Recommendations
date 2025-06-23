namespace Recommendations.Dictionaries.Core.Types;

public sealed class ProductDetails
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    
    public string Gender { get; private set; }
    public string Season { get; private set; }
    public int Year { get; private set; }
    public string Usage { get; private set; }
    
    public string? Description { get; private set; }
    public string? StyleNote { get; private set; }
    public string? MaterialsCare { get; private set; }
    public string? Fit { get; private set; }
    public string? Fabric { get; private set; }
    public string? ArticleNumber { get; private set; }
    public decimal? Vat { get; private set; }
    public string? AgeGroup { get; private set; }
    public string? FashionType { get; private set; }
    public string? LandingPageUrl { get; private set; }
    public string? VariantName { get; private set; }
    public double? MyntraRating { get; private set; }
    public long? CatalogAddDate { get; private set; }
    public string? Colour1 { get; private set; }
    public string? Colour2 { get; private set; }
    public string? VisualTag { get; private set; }
    public string? StyleType { get; private set; }
    public int? ProductTypeId { get; private set; }
    public string? DisplayCategories { get; private set; }
    public string? Weight { get; private set; }
    public int? NavigationId { get; private set; }
    
    public ProductDetails(
        Guid id,
        Guid productId,
        string gender,
        string season,
        int year,
        string usage,
        string? description = null,
        string? styleNote = null,
        string? materialsCare = null,
        string? fit = null,
        string? fabric = null,
        string? articleNumber = null,
        decimal? vat = null,
        string? ageGroup = null,
        string? fashionType = null,
        string? landingPageUrl = null,
        string? variantName = null,
        double? myntraRating = null,
        long? catalogAddDate = null,
        string? colour1 = null,
        string? colour2 = null,
        string? visualTag = null,
        string? styleType = null,
        int? productTypeId = null,
        string? displayCategories = null,
        string? weight = null,
        int? navigationId = null)
    {
        if (string.IsNullOrWhiteSpace(gender)) 
            throw new ArgumentException("Gender cannot be empty", nameof(gender));
        if (string.IsNullOrWhiteSpace(season)) 
            throw new ArgumentException("Season cannot be empty", nameof(season));
        if (year < 1900 || year > 2100) 
            throw new ArgumentException("Year must be between 1900 and 2100", nameof(year));
        if (string.IsNullOrWhiteSpace(usage)) 
            throw new ArgumentException("Usage cannot be empty", nameof(usage));

        Id = id;
        ProductId = productId;
        Gender = gender;
        Season = season;
        Year = year;
        Usage = usage;
        Description = description;
        StyleNote = styleNote;
        MaterialsCare = materialsCare;
        Fit = fit;
        Fabric = fabric;
        ArticleNumber = articleNumber;
        Vat = vat;
        AgeGroup = ageGroup;
        FashionType = fashionType;
        LandingPageUrl = landingPageUrl;
        VariantName = variantName;
        MyntraRating = myntraRating;
        CatalogAddDate = catalogAddDate;
        Colour1 = colour1;
        Colour2 = colour2;
        VisualTag = visualTag;
        StyleType = styleType;
        ProductTypeId = productTypeId;
        DisplayCategories = displayCategories;
        Weight = weight;
        NavigationId = navigationId;
    }

    public static ProductDetails Create(
        Guid productId,
        string gender,
        string season,
        int year,
        string usage,
        string? description = null,
        string? styleNote = null,
        string? materialsCare = null,
        string? fit = null,
        string? fabric = null,
        string? articleNumber = null,
        decimal? vat = null,
        string? ageGroup = null,
        string? fashionType = null,
        string? landingPageUrl = null,
        string? variantName = null,
        double? myntraRating = null,
        long? catalogAddDate = null,
        string? colour1 = null,
        string? colour2 = null,
        string? visualTag = null,
        string? styleType = null,
        int? productTypeId = null,
        string? displayCategories = null,
        string? weight = null,
        int? navigationId = null)
    {
        return new ProductDetails(
            Guid.NewGuid(),
            productId,
            gender,
            season,
            year,
            usage,
            description,
            styleNote,
            materialsCare,
            fit,
            fabric,
            articleNumber,
            vat,
            ageGroup,
            fashionType,
            landingPageUrl,
            variantName,
            myntraRating,
            catalogAddDate,
            colour1,
            colour2,
            visualTag,
            styleType,
            productTypeId,
            displayCategories,
            weight,
            navigationId);
    }
} 