using System.Text.Json.Serialization;

namespace Recommendations.Dictionaries.Infrastructure.Services.ImportDataset.FashionDataset;

public class ProductJsonResponse
{
    public ProductJsonData? Data { get; set; }
}

public class ProductJsonData
{
    public int Id { get; set; }
    public decimal Price { get; }
    public decimal DiscountedPrice { get; }
    public string BrandName => string.Empty;
    public string ProductDisplayName => string.Empty;
    public string Season => string.Empty;
    public string Year => string.Empty;
    public string Usage => string.Empty;
    public string BaseColour => string.Empty;
    public string StyleType => string.Empty;
    public string FashionType => string.Empty;
    public string AgeGroup => string.Empty;
    public string ArticleNumber => string.Empty;
    public decimal? Vat { get; }
    public string LandingPageUrl => string.Empty;
    public string VariantName => string.Empty;
    public double? MyntraRating { get; }
    public long? CatalogAddDate { get; }
    public string Colour1 => string.Empty;
    public string Colour2 => string.Empty;
    public string VisualTag => string.Empty;
    public string DisplayCategories => string.Empty;
    public string Weight => string.Empty;
    public int? NavigationId { get; }
    public string Gender => string.Empty;
    public ProductDescriptors? ProductDescriptors { get; }
    public StyleImages? StyleImages { get; }
    public ArticleAttributes? ArticleAttributes { get; }
    public CategoryInfo? MasterCategory { get; }
    public CategoryInfo? SubCategory { get; }
    public CategoryInfo? ArticleType { get; }
}

public class ProductDescriptors
{
    public Description? Description { get; }
    public StyleNote? StyleNote { get; }
    public MaterialsCareDesc? MaterialsCareDesc { get; }
}

public class Description
{
    public string Value => string.Empty;
}

public class StyleNote
{
    public string Value => string.Empty;
}

public class MaterialsCareDesc
{
    public string Value => string.Empty;
}

public class StyleImages
{
    public ImageInfo? Default { get; }
    public ImageInfo? Front { get; }
    public ImageInfo? Back { get; }
    public ImageInfo? Left { get; set; }
    public ImageInfo? Right { get; set; }
    public ImageInfo? Search { get; set; }
}

public class ImageInfo
{
    public string ImageUrl => string.Empty;
    public string ImageType => string.Empty;
    public Dictionary<string, string>? Resolutions { get; set; }
}

public class ArticleAttributes
{
    public string? Fit { get; }
    public string? Pattern { get; set; }
    [JsonPropertyName("Body or Garment Size")]
    public string? BodyOrGarmentSize { get; set; }
    [JsonPropertyName("Sleeve Length")]
    public string? SleeveLength { get; set; }
    public string? Collar { get; set; }
    public string? Fabric { get; }
    public string? Neck { get; set; }
    public string? Occasion { get; set; }
    public string? Type { get; set; }
    public string? Closure { get; set; }
    public string? WaistRise { get; set; }
    public string? Length { get; set; }
    public string? Rise { get; set; }
    public string? Waistband { get; set; }
    public string? Pockets { get; set; }
    public string? Cuff { get; set; }
    public string? Placket { get; set; }
    public string? Hem { get; set; }
    public string? Sleeve { get; set; }
    public string? Neckline { get; set; }
    public string? Strap { get; set; }
    public string? Sole { get; set; }
    public string? Toe { get; set; }
    public string? Heel { get; set; }
    public string? Ankle { get; set; }
    public string? Lining { get; set; }
    public string? Outer { get; set; }
    public string? Inner { get; set; }
    public string? Fastening { get; set; }
    public string? StrapType { get; set; }
    public string? Movement { get; set; }
    public string? WaterResistant { get; set; }
    public string? Case { get; set; }
    public string? StrapMaterial { get; set; }
    public string? CaseShape { get; set; }
    public string? CaseSize { get; set; }
    public string? CaseThickness { get; set; }
    public string? CaseMaterial { get; set; }
}

public class CategoryInfo
{
    public int Id { get; set; }
    public string TypeName => string.Empty;
    public bool Active { get; set; }
} 