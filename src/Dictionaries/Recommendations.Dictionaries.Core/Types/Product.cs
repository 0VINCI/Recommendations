namespace Recommendations.Dictionaries.Core.Types;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string ProductDisplayName { get; private set; }
    public string BrandName { get; private set; }
    public decimal Price { get; private set; }
    public decimal? OriginalPrice { get; private set; }
    public decimal Rating { get; private set; }
    public int Reviews { get; private set; }
    public bool IsBestseller { get; private set; }
    public bool IsNew { get; private set; }
    
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
        string productDisplayName,
        string brandName,
        decimal price,
        decimal? originalPrice,
        decimal rating,
        int reviews,
        bool isBestseller,
        bool isNew,
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
        ProductDisplayName = productDisplayName;
        BrandName = brandName;
        Price = price;
        OriginalPrice = originalPrice;
        Rating = rating;
        Reviews = reviews;
        IsBestseller = isBestseller;
        IsNew = isNew;
        SubCategoryId = subCategoryId;
        ArticleTypeId = articleTypeId;
        BaseColourId = baseColourId;
    }

    public static Product Create(
        string productDisplayName,
        string brandName,
        decimal price,
        decimal? originalPrice,
        decimal rating,
        int reviews,
        bool isBestseller,
        bool isNew,
        Guid subCategoryId,
        Guid articleTypeId,
        Guid baseColourId)
    {
        return new Product(
            Guid.NewGuid(),
            productDisplayName,
            brandName,
            price,
            originalPrice,
            rating,
            reviews,
            isBestseller,
            isNew,
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

    public void UpdatePrice(decimal newPrice, decimal? newOriginalPrice = null)
    {
        if (newPrice < 0) 
            throw new ArgumentException("Price must be >= 0", nameof(newPrice));

        Price = newPrice;
        OriginalPrice = newOriginalPrice;
    }
} 