using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Purchase.Core;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Purchase.Shared.Queries;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations.Purchase.Api;

internal sealed class PurchaseModule : ModuleDefinition
{
    public override string ModulePrefix => "/orders";
    public override bool RequireAuthorization => true;
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/create", async (
            [FromBody] CreateOrder command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });
        
        app.MapGet("/mine", async (
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) 
            => Results.Ok(await queryDispatcher.QueryAsync(new GetCustomerOrders(), cancellationToken)));

        app.MapGet("/{orderId}", async (
            [FromRoute] Guid orderId,
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) => 
            Results.Ok(await queryDispatcher.QueryAsync(new GetOrderById(orderId), cancellationToken)));

        app.MapPost("/pay", async (//zapłać 
            [FromBody] PayForOrder command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });
        
        app.MapPost("/cancel", async (//anuluj
            [FromBody] CancelOrder command,
            [FromServices] ICommandDispatcher commandDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapGet("/{orderId}/status", async (
            [FromRoute] Guid orderId,
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var result = await queryDispatcher.QueryAsync(new GetOrderStatus(orderId), cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        app.MapPatch("/status", async (
            [FromBody] UpdateStatus command,
            [FromServices] ICommandDispatcher commandDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapGet("/customer", async (
            [FromServices] IQueryDispatcher queryDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            var result = await queryDispatcher.QueryAsync(new GetCustomer(), cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        app.MapPost("/customer", async (
            [FromBody] AddNewCustomer command,
            [FromServices] ICommandDispatcher commandDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPatch("/customer", async (
            [FromBody] UpdateCustomer command,
            [FromServices] ICommandDispatcher commandDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/customer/addresses", async (
            [FromBody] AddNewAddress command,
            [FromServices] ICommandDispatcher commandDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPatch("/customer/address", async (
            [FromBody] UpdateAddress command,
            [FromServices] ICommandDispatcher commandDispatcher,
            CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });
    }
}