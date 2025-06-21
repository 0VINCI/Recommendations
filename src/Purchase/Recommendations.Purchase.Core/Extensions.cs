using Microsoft.Extensions.DependencyInjection;
using Recommendations.Purchase.Core.Commands;
using Recommendations.Purchase.Core.Data;
using Recommendations.Purchase.Core.ModuleApi;
using Recommendations.Purchase.Core.Queries;
using Recommendations.Purchase.Shared;

namespace Recommendations.Purchase.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IPurchaseModuleApi, PurchaseModuleApi>();
        services.AddCommands();
        services.AddQueries();
        services.AddPostgres();

        return services;
    }
}