using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Services;
using Recommendations.Dictionaries.Core.Types;
using Recommendations.Dictionaries.Infrastructure.DAL;

namespace Recommendations.Dictionaries.Infrastructure.Services.ImportDataset.FashionDataset;

public class DataImportService(DictionariesDbContext context) : IDataImportService
{
    private readonly Random _random = new();
    private const int BatchSize = 5000;

    public async Task ImportCsvDataAsync(string stylesCsvPath, string imagesCsvPath)
    {
        await ClearExistingDataAsync();

        var imagesData = await ReadImagesDataAsync(imagesCsvPath);

        await ReadAndProcessStylesDataAsync(stylesCsvPath, imagesData);
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
        await context.Database.CloseConnectionAsync();
        context.ChangeTracker.Clear();
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
                imagesData[filename] = link;
        }

        return imagesData;
    }

    private async Task ReadAndProcessStylesDataAsync(string stylesCsvPath, IReadOnlyDictionary<string, string> imagesData)
    {
        // słowniki globalne (utrzymują referencje i ID między batchami)
        var masterCategories = new Dictionary<string, MasterCategory>();
        var subCategories = new Dictionary<string, SubCategory>();
        var articleTypes = new Dictionary<string, ArticleType>();
        var baseColours = new Dictionary<string, BaseColour>();

        // listy batchowe — zapisujemy i czyścimy co BatchSize produktów
        var batchMasterCategories = new List<MasterCategory>();
        var batchSubCategories = new List<SubCategory>();
        var batchArticleTypes = new List<ArticleType>();
        var batchBaseColours = new List<BaseColour>();
        var batchProducts = new List<Product>();
        var batchProductImages = new List<ProductImage>();

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
            var masterCategory = record.masterCategory?.ToString();
            var subCategory = record.subCategory?.ToString();
            var articleType = record.articleType?.ToString();
            var baseColour = record.baseColour?.ToString();
            var year = record.year?.ToString();
            var usage = record.usage?.ToString();   // celowo nieużywane tutaj – details dodasz z JSON
            var gender = record.gender?.ToString(); // jw.
            var productDisplayName = record.productDisplayName?.ToString();

            if (string.IsNullOrEmpty(productDisplayName) || string.IsNullOrEmpty(id))
                continue;

            // MasterCategory
            if (!string.IsNullOrEmpty(masterCategory) && masterCategory != "NA" && !masterCategories.ContainsKey(masterCategory))
            {
                var mc = MasterCategory.Create(masterCategory);
                masterCategories[masterCategory] = mc;
                batchMasterCategories.Add(mc);
            }

            // SubCategory
            if (!string.IsNullOrEmpty(subCategory) && subCategory != "NA" && !subCategories.ContainsKey(subCategory))
            {
                if (masterCategories.TryGetValue(masterCategory ?? "", out MasterCategory mcEntity))
                {
                    var sc = SubCategory.Create(subCategory, mcEntity.Id);
                    subCategories[subCategory] = sc;
                    batchSubCategories.Add(sc);
                }
            }

            // ArticleType
            if (!string.IsNullOrEmpty(articleType) && articleType != "NA" && !articleTypes.ContainsKey(articleType))
            {
                if (subCategories.TryGetValue(subCategory ?? "", out SubCategory scEntity))
                {
                    var at = ArticleType.Create(articleType, scEntity.Id);
                    articleTypes[articleType] = at;
                    batchArticleTypes.Add(at);
                }
            }

            // BaseColour
            if (!string.IsNullOrEmpty(baseColour) && baseColour != "NA" && !baseColours.ContainsKey(baseColour))
            {
                var bc = BaseColour.Create(baseColour);
                baseColours[baseColour] = bc;
                batchBaseColours.Add(bc);
            }

            // walidacja powiązań
            if (!subCategories.TryGetValue(subCategory ?? "", out SubCategory finalSubCategory)) continue;
            if (!articleTypes.TryGetValue(articleType ?? "", out ArticleType finalArticleType)) continue;
            if (!baseColours.TryGetValue(baseColour ?? "", out BaseColour finalBaseColour)) continue;

            // dane sprzedażowe (syntetyczne)
            var priceTuple = GeneratePrice(masterCategory ?? string.Empty, finalArticleType.Name);
            decimal price = priceTuple.Item1;
            decimal? originalPrice = priceTuple.Item2;

            var ratingReviews = GenerateRatingAndReviews();
            decimal rating = ratingReviews.Item1;
            int reviews = ratingReviews.Item2;

            var isBestseller = GenerateBestsellerStatus(rating, reviews);
            var isNew = GenerateNewStatus(int.TryParse(year, out int y) ? y : 2020);

            // Product (ExternalId = id z CSV)
            var product = Product.Create(
                id, productDisplayName, GenerateBrandName(finalArticleType.Name),
                price, originalPrice,
                rating, reviews, isBestseller, isNew,
                finalSubCategory.Id, finalArticleType.Id, finalBaseColour.Id
            );
            batchProducts.Add(product);

            // Zdjęcie główne (jeśli jest)
            if (imagesData.TryGetValue(id, out string imageUrl))
                batchProductImages.Add(ProductImage.Create(product.Id, imageUrl, "default", null, true));

            // flush co BatchSize produktów
            if (batchProducts.Count >= BatchSize)
                await FlushCsvBatchAsync(
                    batchMasterCategories, batchSubCategories, batchArticleTypes, batchBaseColours,
                    batchProducts, batchProductImages
                );
        }

        // flush resztki
        if (batchProducts.Count > 0 ||
            batchMasterCategories.Count > 0 || batchSubCategories.Count > 0 ||
            batchArticleTypes.Count > 0 || batchBaseColours.Count > 0 ||
            batchProductImages.Count > 0)
        {
            await FlushCsvBatchAsync(
                batchMasterCategories, batchSubCategories, batchArticleTypes, batchBaseColours,
                batchProducts, batchProductImages
            );
        }
    }

    private async Task FlushCsvBatchAsync(
        List<MasterCategory> batchMasterCategories,
        List<SubCategory> batchSubCategories,
        List<ArticleType> batchArticleTypes,
        List<BaseColour> batchBaseColours,
        List<Product> batchProducts,
        List<ProductImage> batchProductImages)
    {
        if (batchMasterCategories.Count > 0) await context.MasterCategories.AddRangeAsync(batchMasterCategories);
        if (batchSubCategories.Count > 0)   await context.SubCategories.AddRangeAsync(batchSubCategories);
        if (batchArticleTypes.Count > 0)    await context.ArticleTypes.AddRangeAsync(batchArticleTypes);
        if (batchBaseColours.Count > 0)     await context.BaseColours.AddRangeAsync(batchBaseColours);
        if (batchProducts.Count > 0)        await context.Products.AddRangeAsync(batchProducts);
        if (batchProductImages.Count > 0)   await context.ProductImages.AddRangeAsync(batchProductImages);

        await context.SaveChangesAsync();

        // „odśwież” kontekst i połączenie
        await context.Database.CloseConnectionAsync();
        context.ChangeTracker.Clear();

        // wyczyść batch
        batchMasterCategories.Clear();
        batchSubCategories.Clear();
        batchArticleTypes.Clear();
        batchBaseColours.Clear();
        batchProducts.Clear();
        batchProductImages.Clear();
    }

    public async Task ImportJsonDataAsync(string jsonDirectoryPath)
    {
        var jsonFiles = Directory.GetFiles(jsonDirectoryPath, "*.json");
        if (jsonFiles.Length == 0) return;

        var batchDetails = new List<ProductDetails>();
        var batchImages = new List<ProductImage>();
        int processed = 0;

        foreach (var jsonFile in jsonFiles)
        {
            var jsonContent = await File.ReadAllTextAsync(jsonFile);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jsonResponse = JsonSerializer.Deserialize<ProductJsonResponse>(jsonContent, options);

            if (jsonResponse?.Data == null) continue;
            var productData = jsonResponse.Data;

            // znajdź produkt po ExternalId == Id z JSON
            var existingProduct = await context
                .Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ExternalId == productData.Id.ToString());

            if (existingProduct == null) continue;

            // details: dodaj tylko jeśli brak
            var hasDetails = await context.ProductDetails
                .AsNoTracking()
                .AnyAsync(pd => pd.ProductId == existingProduct.Id);

            if (!hasDetails)
            {
                var productDetail = ProductDetails.Create(
                    existingProduct.Id,
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
                batchDetails.Add(productDetail);
            }

            // obrazy: dodaj tylko jeśli brak jakichkolwiek
            var hasAnyImages = await context.ProductImages
                .AsNoTracking()
                .AnyAsync(pi => pi.ProductId == existingProduct.Id);

            if (!hasAnyImages && productData.StyleImages != null)
            {
                if (productData.StyleImages.Default != null)
                    batchImages.Add(ProductImage.Create(existingProduct.Id, productData.StyleImages.Default.ImageUrl, "default", null, true));
                if (productData.StyleImages.Front != null)
                    batchImages.Add(ProductImage.Create(existingProduct.Id, productData.StyleImages.Front.ImageUrl, "front", null, false));
                if (productData.StyleImages.Back != null)
                    batchImages.Add(ProductImage.Create(existingProduct.Id, productData.StyleImages.Back.ImageUrl, "back", null, false));
            }

            processed++;

            if (processed % BatchSize == 0)
                await FlushJsonBatchAsync(batchDetails, batchImages);
        }

        // flush resztki
        if (batchDetails.Count > 0 || batchImages.Count > 0)
            await FlushJsonBatchAsync(batchDetails, batchImages);
    }

    private async Task FlushJsonBatchAsync(
        List<ProductDetails> batchDetails,
        List<ProductImage> batchImages)
    {
        if (batchDetails.Count > 0) await context.ProductDetails.AddRangeAsync(batchDetails);
        if (batchImages.Count > 0)  await context.ProductImages.AddRangeAsync(batchImages);

        await context.SaveChangesAsync();

        // „odśwież” kontekst i połączenie
        await context.Database.CloseConnectionAsync();
        context.ChangeTracker.Clear();

        batchDetails.Clear();
        batchImages.Clear();
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
        decimal? originalPrice = null;
        if (_random.Next(100) < 30)
        {
            var discountPercentage = _random.Next(10, 40);
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
        var rating = _random.NextDouble() * 1.0 + 4.0;
        var reviews = _random.Next(1, 500);
        return (Math.Round((decimal)rating, 1), reviews);
    }

    private bool GenerateBestsellerStatus(decimal rating, int reviews)
        => rating >= 4.5m && reviews >= 100;

    private bool GenerateNewStatus(int year)
    {
        var currentYear = DateTime.Now.Year;
        return year >= currentYear - 1;
    }

    private static string? GetFabricValue(ArticleAttributes? attributes)
        => attributes?.Fabric ?? attributes?.Fabric2;

    private static string? GetPatternValue(ArticleAttributes? attributes)
        => attributes?.Pattern ?? attributes?.DialPattern;
}
