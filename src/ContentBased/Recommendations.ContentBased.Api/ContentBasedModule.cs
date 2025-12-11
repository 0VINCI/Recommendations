using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.ContentBased.Core;
using Recommendations.ContentBased.Shared;
using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Types;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations.ContentBased.Api;

internal sealed class ContentBasedModule : ModuleDefinition
{
    public override string ModulePrefix => "/content-based";

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddMemoryCache();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/product-embeddings/{productId:guid}/{variant}", async (
            [FromRoute] Guid productId,
            [FromRoute] VectorType variant,
            [FromServices] IContentBasedModuleApi contentBasedModuleApi,
            CancellationToken cancellationToken = default) =>
        {
            var productEmbedding = await contentBasedModuleApi.GetProductEmbedding(productId, variant, cancellationToken);
            return productEmbedding is null ? Results.NotFound() : Results.Ok(productEmbedding);
        });

        app.MapGet("/product-embeddings/{productId:guid}", async (
            [FromRoute] Guid productId,
            [FromServices] IContentBasedModuleApi contentBasedModuleApi,
            CancellationToken cancellationToken = default) =>
        {
            var productEmbeddings = await contentBasedModuleApi.GetProductEmbeddings(productId, cancellationToken);
            return Results.Ok(productEmbeddings);
        });

        app.MapGet("/product-embeddings/{productId:guid}/{variant}/similar", async (
            [FromRoute] Guid productId,
            [FromRoute] VectorType variant,
            [FromQuery] int topCount,
            [FromServices] IContentBasedModuleApi contentBasedModuleApi,
            CancellationToken cancellationToken = default) =>
        {
            topCount = topCount == 0 ? 10 : topCount;

            if (topCount <= 0 || topCount > 100)
                return Results.BadRequest("TopCount must be between 1 and 100");

            var similarProducts = await contentBasedModuleApi.GetSimilarProducts(
                productId, variant, topCount, cancellationToken);
            return Results.Ok(similarProducts);
        });

        app.MapPost("/product-embeddings", async (
            [FromBody] CreateProductEmbeddingDto productEmbedding,
            [FromServices] IContentBasedModuleApi contentBasedModuleApi,
            CancellationToken cancellationToken = default) =>
        {
            await contentBasedModuleApi.CreateProductEmbedding(productEmbedding, cancellationToken);
            return Results.Created($"/content-based/product-embeddings/{productEmbedding.ProductId}/{productEmbedding.Variant}", null);
        });

        app.MapPut("/product-embeddings/{productId:guid}/{variant}", async (
            [FromRoute] Guid productId,
            [FromRoute] VectorType variant,
            [FromBody] UpdateProductEmbeddingDto productEmbedding,
            [FromServices] IContentBasedModuleApi contentBasedModuleApi,
            CancellationToken cancellationToken = default) =>
        {
            await contentBasedModuleApi.UpdateProductEmbedding(productId, variant, productEmbedding, cancellationToken);
            return Results.NoContent();
        });

        app.MapDelete("/product-embeddings/{productId:guid}/{variant}", async (
            [FromRoute] Guid productId,
            [FromRoute] VectorType variant,
            [FromServices] IContentBasedModuleApi contentBasedModuleApi,
            CancellationToken cancellationToken = default) =>
        {
            await contentBasedModuleApi.DeleteProductEmbedding(productId, variant, cancellationToken);
            return Results.NoContent();
        });

        app.MapDelete("/product-embeddings/{productId:guid}", async (
            [FromRoute] Guid productId,
            [FromServices] IContentBasedModuleApi contentBasedModuleApi,
            CancellationToken cancellationToken = default) =>
        {
            await contentBasedModuleApi.DeleteProductEmbeddings(productId, cancellationToken);
            return Results.NoContent();
        });
    }
}