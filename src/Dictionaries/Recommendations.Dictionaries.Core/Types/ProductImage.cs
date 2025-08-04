namespace Recommendations.Dictionaries.Core.Types;

public sealed class ProductImage
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    
    public string ImageUrl { get; private set; }
    public string ImageType { get; private set; }
    public string? Resolution { get; private set; }
    public bool IsPrimary { get; private set; }
    
    public ProductImage(
        Guid id,
        Guid productId,
        string imageUrl,
        string imageType,
        string? resolution = null,
        bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(imageUrl)) 
            throw new ArgumentException("ImageUrl cannot be empty", nameof(imageUrl));
        if (string.IsNullOrWhiteSpace(imageType)) 
            throw new ArgumentException("ImageType cannot be empty", nameof(imageType));

        Id = id;
        ProductId = productId;
        ImageUrl = imageUrl;
        ImageType = imageType;
        Resolution = resolution;
        IsPrimary = isPrimary;
    }

    public static ProductImage Create(
        Guid productId,
        string imageUrl,
        string imageType,
        string? resolution = null,
        bool isPrimary = false)
    {
        return new ProductImage(
            Guid.NewGuid(),
            productId,
            imageUrl,
            imageType,
            resolution,
            isPrimary);
    }
} 