using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Abstractions.Events;
using Recommendations.Tracking.Core.Data;
using Recommendations.Tracking.Core.Events.Handlers;
using Recommendations.Tracking.Core.ModuleApi;
using Recommendations.Tracking.Shared;
using Recommendations.Tracking.Shared.Events;

namespace Recommendations.Tracking.Core.Events;

public static class Extensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddScoped<IEventHandler<CartCleared>, CartClearedHandler>();
        services.AddScoped<IEventHandler<CartItemAdded>, CartItemAddedHandler>();
        services.AddScoped<IEventHandler<CartItemQuantityChanged>, CartItemQuantityChangedHandler>();
        services.AddScoped<IEventHandler<CartItemRemoved>, CartItemRemovedHandler>();
        services.AddScoped<IEventHandler<OrderPaid>, OrderPaidHandler>();
        services.AddScoped<IEventHandler<OrderPlaced>, OrderPlacedHandler>();

        return services;
    }
}