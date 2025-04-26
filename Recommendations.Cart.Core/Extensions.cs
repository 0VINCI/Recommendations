using Microsoft.Extensions.DependencyInjection;
using Recommendations.Cart.Core.ModuleApi;
using Recommendations.Cart.Shared;

namespace Recommendations.Cart.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<ICartModuleApi, CartModuleApi>();

        return services;
    }
}