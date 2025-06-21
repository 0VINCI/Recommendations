using Microsoft.Extensions.DependencyInjection;
using Recommendations.Cart.Application.Commands.Handlers;
using Recommendations.Cart.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Cart.Application.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<AddItemToCart>, AddItemToCartHandler>();
        services.AddScoped<ICommandHandler<ClearCart>, ClearCartHandler>();
        services.AddScoped<ICommandHandler<RemoveItemFromCart>, RemoveItemFromCartHandler>();
        services.AddScoped<ICommandHandler<UpdateCartItemQuantity>, UpdateCartItemQuantityHandler>();

        return services;
    }
}