using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using Recommendations.Dictionaries.Core.Services;
using Recommendations.Dictionaries.Core.Types;
using Recommendations.Dictionaries.Infrastructure.DAL;

namespace Recommendations.Dictionaries.Infrastructure.Services.ImportDataset.FashionDataset;

public class DataImportService(DictionariesDbContext context) : IDataImportService
{
    private readonly Random _random = new();

    public async Task ImportFashionDatasetAsync(string stylesCsvPath, string imagesCsvPath)
    {
        await ClearExistingDataAsync();
        
        var imagesData = await ReadImagesDataAsync(imagesCsvPath);
        
        await ReadAndProcessStylesDataAsync(stylesCsvPath, imagesData);
    }

    public async Task ImportJsonDataAsync(string jsonDirectoryPath)
    {
        await ClearExistingDataAsync();
        
        var masterCategories = new Dictionary<string, MasterCategory>();
        var subCategories = new Dictionary<string, SubCategory>();
        var articleTypes = new Dictionary<string, ArticleType>();
        var baseColours = new Dictionary<string, BaseColour>();
        var products = new List<Product>();
        var productDetails = new List<ProductDetails>();
        var productImages = new List<ProductImage>();

        var jsonFiles = Directory.GetFiles(jsonDirectoryPath, "*.json");
        Console.WriteLine($"Found {jsonFiles.Length} JSON files to process");
        
        var processedCount = 0;
        var errorCount = 0;
        
        foreach (var jsonFile in jsonFiles)
        {
            try
            {
                var jsonContent = await File.ReadAllTextAsync(jsonFile);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var jsonResponse = JsonSerializer.Deserialize<ProductJsonResponse>(jsonContent, options);
                
                if (jsonResponse?.Data == null) 
                {
                    Console.WriteLine($"Skipping {Path.GetFileName(jsonFile)}: no data found");
                    continue;
                }
                
                var productData = jsonResponse.Data;

                // Process MasterCategory
                if (!string.IsNullOrEmpty(productData.MasterCategory?.TypeName) && !masterCategories.ContainsKey(productData.MasterCategory.TypeName))
                {
                    var masterCategoryEntity = MasterCategory.Create(productData.MasterCategory.TypeName);
                    masterCategories[productData.MasterCategory.TypeName] = masterCategoryEntity;
                    Console.WriteLine($"Created MasterCategory: {productData.MasterCategory.TypeName}");
                }

                // Process SubCategory
                if (!string.IsNullOrEmpty(productData.SubCategory?.TypeName) && !subCategories.ContainsKey(productData.SubCategory.TypeName))
                {
                    if (masterCategories.TryGetValue(productData.MasterCategory?.TypeName ?? "", out MasterCategory? mcEntity))
                    {
                        var scEntity = SubCategory.Create(productData.SubCategory.TypeName, mcEntity.Id);
                        subCategories[productData.SubCategory.TypeName] = scEntity;
                        Console.WriteLine($"Created SubCategory: {productData.SubCategory.TypeName}");
                    }
                }

                // Process ArticleType
                if (!string.IsNullOrEmpty(productData.ArticleType?.TypeName) && !articleTypes.ContainsKey(productData.ArticleType.TypeName))
                {
                    if (subCategories.TryGetValue(productData.SubCategory?.TypeName ?? "", out SubCategory? scEntity))
                    {
                        var atEntity = ArticleType.Create(productData.ArticleType.TypeName, scEntity.Id);
                        articleTypes[productData.ArticleType.TypeName] = atEntity;
                        Console.WriteLine($"Created ArticleType: {productData.ArticleType.TypeName}");
                    }
                }

                // Process BaseColour
                if (!string.IsNullOrEmpty(productData.BaseColour) && !baseColours.ContainsKey(productData.BaseColour))
                {
                    var bcEntity = BaseColour.Create(productData.BaseColour);
                    baseColours[productData.BaseColour] = bcEntity;
                    Console.WriteLine($"Created BaseColour: {productData.BaseColour}");
                }

                // Process Product
                if (string.IsNullOrEmpty(productData.ProductDisplayName)) 
                {
                    Console.WriteLine($"Skipping {Path.GetFileName(jsonFile)}: no productDisplayName");
                    continue;
                }

                if (!subCategories.TryGetValue(productData.SubCategory?.TypeName ?? "", out SubCategory? finalSubCategory)) 
                {
                    Console.WriteLine($"Skipping {Path.GetFileName(jsonFile)}: subCategory not found: {productData.SubCategory?.TypeName}");
                    continue;
                }
                if (!articleTypes.TryGetValue(productData.ArticleType?.TypeName ?? "", out ArticleType? finalArticleType)) 
                {
                    Console.WriteLine($"Skipping {Path.GetFileName(jsonFile)}: articleType not found: {productData.ArticleType?.TypeName}");
                    continue;
                }
                if (!baseColours.TryGetValue(productData.BaseColour ?? "", out BaseColour? finalBaseColour)) 
                {
                    Console.WriteLine($"Skipping {Path.GetFileName(jsonFile)}: baseColour not found: {productData.BaseColour}");
                    continue;
                }
                if (finalSubCategory is null || finalArticleType is null || finalBaseColour is null) continue;

                var product = Product.Create(
                    productData.ProductDisplayName,
                    productData.BrandName,
                    productData.Price,
                    productData.DiscountedPrice != productData.Price ? (decimal?)productData.DiscountedPrice : null,
                    (decimal)(productData.MyntraRating ?? 0),
                    0, // reviews - not available in JSON
                    false, // isBestseller - not available in JSON
                    false, // isNew - not available in JSON
                    finalSubCategory.Id,
                    finalArticleType.Id,
                    finalBaseColour.Id
                );

                products.Add(product);

                // Create ProductDetails
                var productDetail = ProductDetails.Create(
                    product.Id,
                    productData.Gender,
                    string.IsNullOrWhiteSpace(productData.Season) ? "All Seasons" : productData.Season,
                    string.IsNullOrWhiteSpace(productData.Usage) ? "Casual" : productData.Usage,
                    productData.Year,
                    productData.ProductDescriptors?.Description?.Value,
                    productData.ArticleAttributes?.SleeveLength,
                    productData.ArticleAttributes?.Fit,
                    GetFabricValue(productData.ArticleAttributes),
                    productData.ArticleAttributes?.Collar,
                    productData.ArticleAttributes?.BodyOrGarmentSize,
                    GetPatternValue(productData.ArticleAttributes),
                    productData.AgeGroup
                );

                product.SetDetails(productDetail);
                productDetails.Add(productDetail);

                // Create ProductImages
                if (productData.StyleImages?.Default != null)
                {
                    var primaryImage = ProductImage.Create(
                        product.Id, 
                        productData.StyleImages.Default.ImageUrl, 
                        productData.StyleImages.Default.ImageType, 
                        null, 
                        true
                    );
                    productImages.Add(primaryImage);
                }

                // Add additional images
                if (productData.StyleImages?.Front != null)
                {
                    var frontImage = ProductImage.Create(
                        product.Id, 
                        productData.StyleImages.Front.ImageUrl, 
                        productData.StyleImages.Front.ImageType, 
                        null, 
                        false
                    );
                    productImages.Add(frontImage);
                }

                if (productData.StyleImages?.Back != null)
                {
                    var backImage = ProductImage.Create(
                        product.Id, 
                        productData.StyleImages.Back.ImageUrl, 
                        productData.StyleImages.Back.ImageType, 
                        null, 
                        false
                    );
                    productImages.Add(backImage);
                }

                processedCount++;
                if (processedCount % 100 == 0)
                {
                    Console.WriteLine($"Processed {processedCount} products...");
                }
            }
            catch (Exception ex)
            {
                errorCount++;
                Console.WriteLine($"Error processing {Path.GetFileName(jsonFile)}: {ex.Message}");
            }
        }

        Console.WriteLine($"Processing complete. Processed: {processedCount}, Errors: {errorCount}");

        // Save to database
        Console.WriteLine("Saving to database...");
        Console.WriteLine($"MasterCategories to save: {masterCategories.Count}");
        Console.WriteLine($"SubCategories to save: {subCategories.Count}");
        Console.WriteLine($"ArticleTypes to save: {articleTypes.Count}");
        Console.WriteLine($"BaseColours to save: {baseColours.Count}");
        Console.WriteLine($"Products to save: {products.Count}");
        Console.WriteLine($"ProductDetails to save: {productDetails.Count}");
        Console.WriteLine($"ProductImages to save: {productImages.Count}");
        
        try
        {
            await context.MasterCategories.AddRangeAsync(masterCategories.Values);
            Console.WriteLine("MasterCategories added to context");
            
            await context.SubCategories.AddRangeAsync(subCategories.Values);
            Console.WriteLine("SubCategories added to context");
            
            await context.ArticleTypes.AddRangeAsync(articleTypes.Values);
            Console.WriteLine("ArticleTypes added to context");
            
            await context.BaseColours.AddRangeAsync(baseColours.Values);
            Console.WriteLine("BaseColours added to context");
            
            await context.Products.AddRangeAsync(products);
            Console.WriteLine("Products added to context");
            
            await context.ProductDetails.AddRangeAsync(productDetails);
            Console.WriteLine("ProductDetails added to context");
            
            await context.ProductImages.AddRangeAsync(productImages);
            Console.WriteLine("ProductImages added to context");
            
            await context.SaveChangesAsync();
            Console.WriteLine($"Import complete. Saved {products.Count} products to database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to database: {ex.Message}");
            Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            throw;
        }
    }

    private async Task ClearExistingDataAsync()
    {
        context.ProductImages.RemoveRange(context.ProductImages);
        context.ProductDetails.RemoveRange(context.ProductDetails);
        context.Products.RemoveRange(context.Products);
        context.ArticleTypes.RemoveRange(context.ArticleTypes);
        context.SubCategories.RemoveRange(context.SubCategories);
        context.MasterCategories.RemoveRange(context.MasterCategories);
        context.BaseColours.RemoveRange(context.BaseColours);
        
        await context.SaveChangesAsync();
    }

    private static async Task<Dictionary<string, string>> ReadImagesDataAsync(string imagesCsvPath)
    {
        var imagesData = new Dictionary<string, string>();
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
            HeaderValidated = null
        };

        using var reader = new StreamReader(imagesCsvPath, System.Text.Encoding.UTF8); 
        using var csv = new CsvReader(reader, config);

        await foreach (var record in csv.GetRecordsAsync<dynamic>())
        {
            var filename = record.filename?.ToString();
            var link = record.link?.ToString();
            
            if (!string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(link))
            {
                imagesData[filename] = link;
            }
        }

        return imagesData;
    }


    private async Task ReadAndProcessStylesDataAsync(string stylesCsvPath, IReadOnlyDictionary<string, string> imagesData)
    {
        var masterCategories = new Dictionary<string, MasterCategory>();
        var subCategories = new Dictionary<string, SubCategory>();
        var articleTypes = new Dictionary<string, ArticleType>();
        var baseColours = new Dictionary<string, BaseColour>();
        var products = new List<Product>();
        var productDetails = new List<ProductDetails>();
        var productImages = new List<ProductImage>();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
            HeaderValidated = null
        };

        using var reader = new StreamReader(stylesCsvPath);
        using var csv = new CsvReader(reader, config);

        await foreach (var record in csv.GetRecordsAsync<dynamic>())
        {
            var id = record.id?.ToString();
            var gender = record.gender?.ToString();
            var masterCategory = record.masterCategory?.ToString();
            var subCategory = record.subCategory?.ToString();
            var articleType = record.articleType?.ToString();
            var baseColour = record.baseColour?.ToString();
            var season = record.season?.ToString();
            var year = record.year?.ToString();
            var usage = record.usage?.ToString();
            var productDisplayName = record.productDisplayName?.ToString();
            
            // Additional fields from JSON - not available in CSV
            var description = (string?)null;
            var fit = (string?)null;
            var fabric = (string?)null;
            var ageGroup = (string?)null;
            var sleeveLength = (string?)null;
            var collar = (string?)null;
            var bodyOrGarmentSize = (string?)null;
            var pattern = (string?)null;
            
            // Process MasterCategory
            if (!string.IsNullOrEmpty(masterCategory) && masterCategory != "NA" && !masterCategories.ContainsKey(masterCategory))
            {
                var masterCategoryEntity = MasterCategory.Create(masterCategory);
                masterCategories[masterCategory] = masterCategoryEntity;
            }

            // Process SubCategory
            if (!string.IsNullOrEmpty(subCategory) && subCategory != "NA" && !subCategories.ContainsKey(subCategory))
            {
                if (masterCategories.TryGetValue(masterCategory ?? "", out MasterCategory? mcEntity))
                {
                    var scEntity = SubCategory.Create(subCategory, mcEntity.Id);
                    subCategories[subCategory] = scEntity;
                }
            }

            // Process ArticleType
            if (!string.IsNullOrEmpty(articleType) && articleType != "NA" && !articleTypes.ContainsKey(articleType))
            {
                if (subCategories.TryGetValue(subCategory ?? "", out SubCategory? scEntity))
                {
                    var atEntity = ArticleType.Create(articleType, scEntity.Id);
                    articleTypes[articleType] = atEntity;
                }
            }

            // Process BaseColour
            if (!string.IsNullOrEmpty(baseColour) && baseColour != "NA" && !baseColours.ContainsKey(baseColour))
            {
                var bcEntity = BaseColour.Create(baseColour);
                baseColours[baseColour] = bcEntity;
            }

            // Process Product
            if (string.IsNullOrEmpty(productDisplayName)) continue;

            if (!subCategories.TryGetValue(subCategory ?? "", out SubCategory? finalSubCategory)) continue;
            if (!articleTypes.TryGetValue(articleType ?? "", out ArticleType? finalArticleType)) continue;
            if (!baseColours.TryGetValue(baseColour ?? "", out BaseColour? finalBaseColour)) continue;
            if (finalSubCategory is null || finalArticleType is null || finalBaseColour is null) continue;

            // Generate realistic e-commerce data
            var priceData = GeneratePrice(masterCategory ?? "", finalArticleType.Name);
            var price = priceData.Item1;
            var originalPrice = priceData.Item2;
            var (rating, reviews) = GenerateRatingAndReviews();
            var isBestseller = GenerateBestsellerStatus(rating, reviews);
            var isNew = GenerateNewStatus(int.TryParse(year, out int yearInt) ? yearInt : 2020);

            var product = Product.Create(
                productDisplayName,
                GenerateBrandName(finalArticleType.Name),
                price,
                originalPrice,
                rating,
                reviews,
                isBestseller,
                isNew,
                finalSubCategory.Id,
                finalArticleType.Id,
                finalBaseColour.Id
            );

            products.Add(product);

            // Create ProductDetails
            var productDetail = ProductDetails.Create(
                product.Id,
                gender ?? "Unisex",
                string.IsNullOrWhiteSpace(season) ? "All Seasons" : season,
                string.IsNullOrWhiteSpace(usage) ? "Casual" : usage,
                description,
                sleeveLength,
                fit,
                fabric,
                collar,
                bodyOrGarmentSize, 
                pattern, 
                ageGroup
            );

            productDetails.Add(productDetail);

            // Create ProductImages
            if (imagesData.TryGetValue(id ?? "", out string? imageUrl))
            {
                var primaryImage = ProductImage.Create(product.Id, imageUrl, "default", null, true);
                productImages.Add(primaryImage);
            }
        }

        // Save to database
        await context.MasterCategories.AddRangeAsync(masterCategories.Values);
        await context.SubCategories.AddRangeAsync(subCategories.Values);
        await context.ArticleTypes.AddRangeAsync(articleTypes.Values);
        await context.BaseColours.AddRangeAsync(baseColours.Values);
        await context.Products.AddRangeAsync(products);
        await context.ProductDetails.AddRangeAsync(productDetails);
        await context.ProductImages.AddRangeAsync(productImages);
        
        await context.SaveChangesAsync();
    }

    private (decimal price, decimal? originalPrice) GeneratePrice(string masterCategory, string articleType)
    {
        var basePrice = masterCategory.ToLower() switch
        {
            "apparel" => _random.Next(20, 200),
            "accessories" => _random.Next(15, 150),
            "footwear" => _random.Next(30, 300),
            "home" => _random.Next(25, 250),
            "personal care" => _random.Next(10, 100),
            "sporting goods" => _random.Next(40, 400),
            _ => _random.Next(20, 150)
        };

        // Adjust price based on article type
        var multiplier = articleType.ToLower() switch
        {
            "watches" => 2.5m,
            "shoes" => 1.8m,
            "jeans" => 1.3m,
            "shirts" => 1.1m,
            "dresses" => 1.4m,
            "bags" => 1.6m,
            _ => 1.0m
        };

        var finalPrice = Math.Round(basePrice * multiplier, 2);
        
        // 30% chance of having an original price (discount)
        var hasOriginalPrice = _random.Next(100) < 30;
        decimal? originalPrice = null;
        
        if (hasOriginalPrice)
        {
            var discountPercentage = _random.Next(10, 40); // 10-40% discount
            originalPrice = Math.Round(finalPrice * (100 + discountPercentage) / 100, 2);
        }

        return (finalPrice, originalPrice);
    }

    private string GenerateBrandName(string articleType)
    {
        var brands = articleType.ToLower() switch
        {
            "watches" => new[] { "Timex", "Casio", "Fossil", "Seiko" },
            "shoes" => new[] { "Nike", "Adidas", "Puma", "Reebok" },
            "jeans" => new[] { "Levi's", "Wrangler", "Lee", "Diesel" },
            "shirts" => new[] { "Tommy Hilfiger", "Calvin Klein", "Ralph Lauren", "Lacoste" },
            "dresses" => new[] { "Zara", "H&M", "Forever 21", "Mango" },
            "bags" => new[] { "Guess", "Fossil", "Michael Kors", "Coach" },
            _ => new[] { "Generic", "Fashion", "Style", "Trend" }
        };

        return brands[_random.Next(brands.Length)];
    }

    private (decimal rating, int reviews) GenerateRatingAndReviews()
    {
        // Generate realistic rating distribution (bell curve around 4.0-4.5)
        var rating = _random.NextDouble() switch
        {
            < 0.05 => _random.NextDouble() * 1.0 + 2.0, // 2.0-3.0 (5%)
            < 0.20 => _random.NextDouble() * 1.0 + 3.0, // 3.0-4.0 (15%)
            < 0.80 => _random.NextDouble() * 1.0 + 4.0, // 4.0-5.0 (60%)
            < 0.95 => _random.NextDouble() * 0.5 + 4.5, // 4.5-5.0 (15%)
            _ => _random.NextDouble() * 0.5 + 4.5 // 4.5-5.0 (5%)
        };

        // Generate realistic number of reviews
        var reviews = _random.NextDouble() switch
        {
            < 0.40 => _random.Next(1, 10),      // 1-10 reviews (40%)
            < 0.70 => _random.Next(10, 50),     // 10-50 reviews (30%)
            < 0.90 => _random.Next(50, 200),    // 50-200 reviews (20%)
            < 0.98 => _random.Next(200, 1000),  // 200-1000 reviews (8%)
            _ => _random.Next(1000, 5000)       // 1000-5000 reviews (2%)
        };

        return (Math.Round((decimal)rating, 1), reviews);
    }

    private bool GenerateBestsellerStatus(decimal rating, int reviews)
    {
        // Product is bestseller if it has high rating AND many reviews
        var isHighRated = rating >= 4.5m;
        var hasManyReviews = reviews >= 100;
        var randomFactor = _random.Next(100) < 5; // 5% random chance

        return (isHighRated && hasManyReviews) || randomFactor;
    }

    private bool GenerateNewStatus(int year)
    {
        var currentYear = DateTime.Now.Year;
        var isRecent = year >= currentYear - 1;
        var randomFactor = _random.Next(100) < 10; // 10% random chance for older items

        return isRecent || randomFactor;
    }

    private static string? GetFabricValue(ArticleAttributes? attributes)
    {
        if (attributes == null) return null;
        
        // Sprawdź różne warianty pola Fabric
        if (!string.IsNullOrWhiteSpace(attributes.Fabric) && attributes.Fabric != "NA")
            return attributes.Fabric;
        if (!string.IsNullOrWhiteSpace(attributes.Fabric2) && attributes.Fabric2 != "NA")
            return attributes.Fabric2;
        
        return null;
    }

    private static string? GetPatternValue(ArticleAttributes? attributes)
    {
        if (attributes == null) return null;
        
        // Sprawdź różne warianty pola Pattern
        if (!string.IsNullOrWhiteSpace(attributes.Pattern) && attributes.Pattern != "NA")
            return attributes.Pattern;
        if (!string.IsNullOrWhiteSpace(attributes.DialPattern) && attributes.DialPattern != "NA")
            return attributes.DialPattern;
        
        return null;
    }
} 