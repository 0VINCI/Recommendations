using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Shared;
using Recommendations.Shared.ModuleDefinition;
using Recommendations.VisualBased.Core;
using Recommendations.VisualBased.Core.Repositories;
using Recommendations.VisualBased.Shared;

namespace Recommendations.VisualBased.Api;

public class VisualBasedModule : ModuleDefinition
{
    public override string ModulePrefix => "/visual";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/similar-items/{productId:guid}/products", async (
            [FromRoute] Guid productId,
            [FromQuery] int? topCount,
            [FromServices] IVisualBasedModuleApi visualBasedModuleApi,
            [FromServices] IVisualEmbeddingRepository visualEmbeddingRepository,
            CancellationToken cancellationToken = default) =>
        {
            var count = topCount ?? 10;
            if (count is <= 0 or > 100)
                return Results.BadRequest(new { error = "topCount must be between 1 and 100" });

            try
            {
                var productIdString = productId.ToString();
                var exists = await visualEmbeddingRepository.ItemEmbeddingExists(productIdString, cancellationToken);
                if (!exists)
                    return Results.NotFound(new { error = $"No visual embedding found for product {productId}" });

                var products = await visualBasedModuleApi.GetSimilarProducts(productId, count, cancellationToken);
                
                return !products.Any() ? Results.NotFound(new { error = "No similar products found" }) : Results.Ok(products);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to get similar products: {ex.Message}");
            }
        });
    }
}