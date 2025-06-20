﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Cart.Application;
using Recommendations.Cart.Core;
using Recommendations.Cart.Infrastructure;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Cart.Shared.Queries;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations.Cart.Api;

internal sealed class CartModule : ModuleDefinition
{
    public override string ModulePrefix => "/cart";
    public override bool RequireAuthorization => true;
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddApplication();
        services.AddInfrastructure();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/addItem", async (
            [FromBody] AddItemToCart command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });
        
        app.MapPost("/removeItem", async (
            [FromBody] RemoveItemFromCart command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/updateQuantity", async (
            [FromBody] UpdateCartItemQuantity command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/clearCart", async (
            [FromBody] ClearCart command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/getCartItems", async (
            [FromBody] GetCart query,
            [FromServices] IQueryDispatcher queryDispatcher, CancellationToken cancellationToken = default) =>
        {
            var cart = await queryDispatcher.QueryAsync(new GetCart(query.CartId), cancellationToken);
            return cart is null ? Results.NotFound() : Results.Ok(cart);
        });

        app.MapGet("/user", async (
            [FromServices] IQueryDispatcher queryDispatcher, CancellationToken cancellationToken = default) =>
        {
            var cart = await queryDispatcher.QueryAsync(new GetUserCart(), cancellationToken);
            return cart is null ? Results.NotFound() : Results.Ok(cart);
        });
    }
}