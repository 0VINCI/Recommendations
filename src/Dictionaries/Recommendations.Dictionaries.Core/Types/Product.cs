namespace Recommendations.Dictionaries.Core.Types;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public decimal? OriginalPrice { get; private set; }
    public string Image { get; private set; }
    public string Category { get; private set; }
    public string Description { get; private set; }
    public IReadOnlyCollection<string>? Sizes { get; private set; }
    public IReadOnlyCollection<string> Colors { get; private set; }
    public decimal Rating { get; private set; }
    public int Reviews { get; private set; }
    public bool IsBestseller { get; private set; }
    public bool IsNew { get; private set; }

    public Product(
        Guid id,
        string name,
        decimal price,
        decimal? originalPrice,
        string image,
        string category,
        string description,
        IReadOnlyCollection<string>? sizes,
        IReadOnlyCollection<string>? colors,
        decimal rating,
        int reviews,
        bool isBestseller = false,
        bool isNew = false)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", nameof(name));
        if (price < 0) throw new ArgumentException("Price must be >= 0", nameof(price));
        if (string.IsNullOrWhiteSpace(image)) throw new ArgumentException("Image cannot be empty", nameof(image));
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category cannot be empty", nameof(category));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be empty", nameof(description));
        if (rating is < 0 or > 5) throw new ArgumentException("Rating must be between 0 and 5", nameof(rating));
        if (reviews < 0) throw new ArgumentException("Reviews must be >= 0", nameof(reviews));

        Id = id;
        Name = name;
        Price = price;
        OriginalPrice = originalPrice;
        Image = image;
        Category = category;
        Description = description;
        Sizes = sizes ?? Array.Empty<string>();
        Colors = colors ?? Array.Empty<string>();
        Rating = rating;
        Reviews = reviews;
        IsBestseller = isBestseller;
        IsNew = isNew;
    }

    public static Product Create(
        string name,
        decimal price,
        decimal? originalPrice,
        string image,
        string category,
        string description,
        IReadOnlyCollection<string>? sizes,
        IReadOnlyCollection<string>? colors,
        decimal rating = 0,
        int reviews = 0,
        bool isBestseller = false,
        bool isNew = false)
    {
        return new Product(
            Guid.NewGuid(),
            name,
            price,
            originalPrice,
            image,
            category,
            description,
            sizes,
            colors,
            rating,
            reviews,
            isBestseller,
            isNew);
    }

    public void UpdateRating(decimal newRating, int newReviews)
    {
        if (newRating < 0 || newRating > 5) throw new ArgumentException("Rating must be between 0 and 5", nameof(newRating));
        if (newReviews < 0) throw new ArgumentException("Reviews must be >= 0", nameof(newReviews));

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
} 