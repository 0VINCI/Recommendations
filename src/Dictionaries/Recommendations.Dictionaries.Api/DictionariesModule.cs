using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Application;
using Recommendations.Dictionaries.Core;
using Recommendations.Dictionaries.Infrastructure;
using Recommendations.Dictionaries.Shared.Commands;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations.Dictionaries.Api;

internal sealed class DictionariesModule : ModuleDefinition
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
            var products = await queryDispatcher.QueryAsync(new GetAllProducts(), cancellationToken);
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
    }
}