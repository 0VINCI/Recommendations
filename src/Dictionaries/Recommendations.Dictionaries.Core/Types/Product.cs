namespace Recommendations.Dictionaries.Core.Types;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string? ExternalId { get; private set; }
    public string ProductDisplayName { get; private set; }
    public string BrandName { get; private set; }
    public decimal Price { get; private set; }
    public decimal? OriginalPrice { get; private set; }
    public decimal Rating { get; private set; }
    public int Reviews { get; private set; }
    public bool IsBestseller { get; private set; }
    public bool IsNew { get; private set; }
    public bool IsTrending { get; private set; }
    public bool IsOnSale { get; private set; }
    public bool ProfitBoost { get; private set; }
    
    public Guid SubCategoryId { get; private set; }
    public SubCategory SubCategory { get; private set; } = null!;
    public Guid ArticleTypeId { get; private set; }
    public ArticleType ArticleType { get; private set; } = null!;
    public Guid BaseColourId { get; private set; }
    public BaseColour BaseColour { get; private set; } = null!;
    
    public ProductDetails? Details { get; private set; }
    public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();
    
    public Product(
        Guid id,
        string? externalId,
        string productDisplayName,
        string brandName,
        decimal price,
        decimal? originalPrice,
        decimal rating,
        int reviews,
        bool isBestseller,
        bool isNew,
        bool isTrending,
        bool isOnSale,
        bool profitBoost,
        Guid subCategoryId,
        Guid articleTypeId,
        Guid baseColourId)
    {
        if (string.IsNullOrWhiteSpace(productDisplayName)) 
            throw new ArgumentException("ProductDisplayName cannot be empty", nameof(productDisplayName));
        if (string.IsNullOrWhiteSpace(brandName)) 
            throw new ArgumentException("BrandName cannot be empty", nameof(brandName));
        if (price < 0) 
            throw new ArgumentException("Price must be >= 0", nameof(price));
        if (rating is < 0 or > 5) 
            throw new ArgumentException("Rating must be between 0 and 5", nameof(rating));
        if (reviews < 0) 
            throw new ArgumentException("Reviews must be >= 0", nameof(reviews));

        Id = id;
        ExternalId = externalId;
        ProductDisplayName = productDisplayName;
        BrandName = brandName;
        Price = price;
        OriginalPrice = originalPrice;
        Rating = rating;
        Reviews = reviews;
        IsBestseller = isBestseller;
        IsNew = isNew;
        IsTrending = isTrending;
        IsOnSale = isOnSale;
        ProfitBoost = profitBoost;
        SubCategoryId = subCategoryId;
        ArticleTypeId = articleTypeId;
        BaseColourId = baseColourId;
    }

    public static Product Create(
        string? externalId,
        string productDisplayName,
        string brandName,
        decimal price,
        decimal? originalPrice,
        decimal rating,
        int reviews,
        bool isBestseller,
        bool isNew,
        bool isTrending,
        bool isOnSale,
        bool profitBoost,
        Guid subCategoryId,
        Guid articleTypeId,
        Guid baseColourId)
    {
        return new Product(
            Guid.NewGuid(),
            externalId,
            productDisplayName,
            brandName,
            price,
            originalPrice,
            rating,
            reviews,
            isBestseller,
            isNew,
            isTrending,
            isOnSale,
            profitBoost,
            subCategoryId,
            articleTypeId,
            baseColourId);
    }

    public void UpdateRating(decimal newRating, int newReviews)
    {
        if (newRating is < 0 or > 5) 
            throw new ArgumentException("Rating must be between 0 and 5", nameof(newRating));
        if (newReviews < 0) 
            throw new ArgumentException("Reviews must be >= 0", nameof(newReviews));

        Rating = newRating;
        Reviews = newReviews;
    }

    public void MarkAsBestseller(bool isBestseller)
    {
        IsBestseller = isBestseller;
    }

    public void MarkAsNew(bool isNew)
    {
        IsNew = isNew;
    }

    public void MarkAsTrending(bool isTrending)
    {
        IsTrending = isTrending;
    }

    public void MarkAsOnSale(bool isOnSale)
    {
        IsOnSale = isOnSale;
    }

    public void MarkAsProfitBoost(bool profitBoost)
    {
        ProfitBoost = profitBoost;
    }

    public void UpdatePrice(decimal newPrice, decimal? newOriginalPrice = null)
    {
        if (newPrice < 0) 
            throw new ArgumentException("Price must be >= 0", nameof(newPrice));

        Price = newPrice;
        OriginalPrice = newOriginalPrice;
    }

    public void SetDetails(ProductDetails details)
    {
        Details = details;
    }

    public void UpdateFromJson(
        string brandName,
        decimal price,
        decimal? originalPrice,
        decimal rating,
        Guid subCategoryId,
        Guid articleTypeId,
        Guid baseColourId)
    {
        if (string.IsNullOrWhiteSpace(brandName)) 
            throw new ArgumentException("BrandName cannot be empty", nameof(brandName));
        if (price < 0) 
            throw new ArgumentException("Price must be >= 0", nameof(price));
        if (rating is < 0 or > 5) 
            throw new ArgumentException("Rating must be between 0 and 5", nameof(rating));

        BrandName = brandName;
        Price = price;
        OriginalPrice = originalPrice;
        Rating = rating;
        SubCategoryId = subCategoryId;
        ArticleTypeId = articleTypeId;
        BaseColourId = baseColourId;
    }
} 