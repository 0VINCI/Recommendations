using System.Text.Json.Serialization;

namespace Recommendations.Dictionaries.Infrastructure.Services.ImportDataset.FashionDataset;

public class ProductJsonResponse
{
    public ProductJsonData? Data { get; set; }
}

public class ProductJsonData
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ProductDisplayName { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Usage { get; set; } = string.Empty;
    public string BaseColour { get; set; } = string.Empty;
    public string StyleType { get; set; } = string.Empty;
    public string FashionType { get; set; } = string.Empty;
    public string AgeGroup { get; set; } = string.Empty;
    public string ArticleNumber { get; set; } = string.Empty;
    public decimal? Vat { get; set; }
    public string LandingPageUrl { get; set; } = string.Empty;
    public string VariantName { get; set; } = string.Empty;
    public double? MyntraRating { get; set; }
    public long? CatalogAddDate { get; set; }
    public string Colour1 { get; set; } = string.Empty;
    public string Colour2 { get; set; } = string.Empty;
    public string VisualTag { get; set; } = string.Empty;
    public string DisplayCategories { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public int? NavigationId { get; set; }
    public string Gender { get; set; } = string.Empty;
    public ProductDescriptors? ProductDescriptors { get; set; }
    public StyleImages? StyleImages { get; set; }
    public ArticleAttributes? ArticleAttributes { get; set; }
    public CategoryInfo? MasterCategory { get; set; }
    public CategoryInfo? SubCategory { get; set; }
    public CategoryInfo? ArticleType { get; set; }
}

public class ProductDescriptors
{
    public Description? Description { get; set; }
    public StyleNote? StyleNote { get; set; }
    public MaterialsCareDesc? MaterialsCareDesc { get; set; }
}

public class Description
{
    public string Value { get; set; } = string.Empty;
}

public class StyleNote
{
    public string Value { get; set; } = string.Empty;
}

public class MaterialsCareDesc
{
    public string Value { get; set; } = string.Empty;
}

public class StyleImages
{
    public ImageInfo? Default { get; set; }
    public ImageInfo? Front { get; set; }
    public ImageInfo? Back { get; set; }
    public ImageInfo? Left { get; set; }
    public ImageInfo? Right { get; set; }
    public ImageInfo? Search { get; set; }
}

public class ImageInfo
{
    public string ImageUrl { get; set; } = string.Empty;
    public string ImageType { get; set; } = string.Empty;
    public Dictionary<string, string>? Resolutions { get; set; }
}

public class ArticleAttributes
{
    public string? Fit { get; set; }
    public string? Pattern { get; set; }
    [JsonPropertyName("Dial Pattern")]
    public string? DialPattern { get; set; }
    [JsonPropertyName("Body or Garment Size")]
    public string? BodyOrGarmentSize { get; set; }
    [JsonPropertyName("Sleeve Length")]
    public string? SleeveLength { get; set; }
    public string? Collar { get; set; }
    public string? Fabric { get; set; }
    [JsonPropertyName("Fabric 2")]
    public string? Fabric2 { get; set; }
}

public class CategoryInfo
{
    public int Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public bool Active { get; set; }
} 