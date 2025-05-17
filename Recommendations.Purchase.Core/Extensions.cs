using Microsoft.Extensions.DependencyInjection;
using Recommendations.Purchase.Core.Data;
using Recommendations.Purchase.Core.ModuleApi;
using Recommendations.Purchase.Shared;

namespace Recommendations.Purchase.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IPurchaseModuleApi, PurchaseModuleApi>();
        services.AddPostgres();

        return services;
    }
}