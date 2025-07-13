using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Application;
using Recommendations.Dictionaries.Core;
using Recommendations.Dictionaries.Core.Services;
using Recommendations.Dictionaries.Infrastructure;
using Recommendations.Dictionaries.Infrastructure.DAL;
using Recommendations.Dictionaries.Shared.Commands;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations.Dictionaries.Api;

internal sealed class 
    DictionariesModule : ModuleDefinition
{
    public override string ModulePrefix => "/dic";

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddInfrastructure();
        services.AddApplication();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var products = await queryDispatcher.QueryAsync(new GetProducts(), cancellationToken);
            return Results.Ok(products);
        });

        app.MapGet("/products/{id}", async (
            [FromRoute] Guid id,
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var product = await queryDispatcher.QueryAsync(new GetProductById(id), cancellationToken);
            return product is null ? Results.NotFound() : Results.Ok(product);
        });

        app.MapGet("/products/category/{category}", async (
            [FromRoute] string category,
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var products = await queryDispatcher.QueryAsync(new GetProductsByCategory(category), cancellationToken);
            return Results.Ok(products);
        });

        app.MapGet("/products/bestsellers", async (
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var products = await queryDispatcher.QueryAsync(new GetBestsellers(), cancellationToken);
            return Results.Ok(products);
        });

        app.MapGet("/products/new", async (
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var products = await queryDispatcher.QueryAsync(new GetNewProducts(), cancellationToken);
            return Results.Ok(products);
        });

        app.MapGet("/products/search", async (
            [FromQuery] string query,
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var products = await queryDispatcher.QueryAsync(new SearchProducts(query), cancellationToken);
            return Results.Ok(products);
        });

        app.MapPost("/products", async (
            [FromBody] ProductDto productDto,
            [FromServices] ICommandDispatcher commandDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(new AddProduct(productDto), cancellationToken);
            return Results.StatusCode(StatusCodes.Status201Created);
        });

        app.MapPut("/products/{id}", async (
            [FromRoute] Guid id,
            [FromBody] ProductDto productDto,
            [FromServices] ICommandDispatcher commandDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var updatedProduct = productDto with { Id = id };
            await commandDispatcher.SendAsync(new UpdateProduct(updatedProduct), cancellationToken);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapDelete("/products/{id}", async (
            [FromRoute] Guid id,
            [FromServices] ICommandDispatcher commandDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(new DeleteProduct(id), cancellationToken);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/import/fashion-dataset", async (
            [FromServices] IDataImportService dataImportService,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var stylesPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "fashion-dataset", "styles.csv");
                var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "fashion-dataset", "images.csv");
                
                await dataImportService.ImportFashionDatasetAsync(stylesPath, imagesPath);
                return Results.Ok(new { message = "Fashion dataset imported successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        app.MapGet("/import/stats", async (
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var products = await queryDispatcher.QueryAsync(new GetAllProducts(), cancellationToken);
                
                if (!products.Any())
                {
                    return Results.Ok(new { message = "No products found. Import data first." });
                }

                var stats = new
                {
                    TotalProducts = products.Count(),
                    PriceStats = new
                    {
                        AveragePrice = Math.Round(products.Average(p => p.Price), 2),
                        MinPrice = products.Min(p => p.Price),
                        MaxPrice = products.Max(p => p.Price),
                        ProductsWithDiscount = products.Count(p => p.OriginalPrice.HasValue)
                    },
                    RatingStats = new
                    {
                        AverageRating = Math.Round(products.Average(p => p.Rating), 1),
                        MinRating = products.Min(p => p.Rating),
                        MaxRating = products.Max(p => p.Rating),
                        HighRatedProducts = products.Count(p => p.Rating >= 4.5m)
                    },
                    ReviewsStats = new
                    {
                        AverageReviews = Math.Round(products.Average(p => p.Reviews), 0),
                        MinReviews = products.Min(p => p.Reviews),
                        MaxReviews = products.Max(p => p.Reviews),
                        PopularProducts = products.Count(p => p.Reviews >= 100)
                    },
                    StatusStats = new
                    {
                        Bestsellers = products.Count(p => p.IsBestseller),
                        NewProducts = products.Count(p => p.IsNew)
                    },
                    CategoryStats = products.GroupBy(p => p.SubCategoryName)
                        .Select(g => new { Category = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                };

                return Results.Ok(stats);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        app.MapDelete("/import/clear", async (
            [FromServices] DictionariesDbContext context,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                context.Products.RemoveRange(context.Products);
                context.BaseColours.RemoveRange(context.BaseColours);
                context.ArticleTypes.RemoveRange(context.ArticleTypes);
                context.SubCategories.RemoveRange(context.SubCategories);
                
                await context.SaveChangesAsync(cancellationToken);
                
                return Results.Ok(new { message = "All data cleared successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
        
        //endpoint about needing to import one particular set of data - that's why messy :)
        app.MapPost("/import/json-data", async (
            [FromServices] IDataImportService dataImportService,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "fashion-dataset", "styles");
                
                if (!Directory.Exists(jsonPath))
                {
                    return Results.BadRequest(new { error = "JSON directory not found. Expected path: " + jsonPath });
                }
                
                await dataImportService.ImportJsonDataAsync(jsonPath);
                return Results.Ok(new { message = "JSON data imported successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        app.MapGet("/products/{id}/full", async (
            [FromRoute] Guid id,
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var product = await queryDispatcher.QueryAsync(new GetProductFullById(id), cancellationToken);
            return product is null ? Results.NotFound() : Results.Ok(product);
        });
    }
}