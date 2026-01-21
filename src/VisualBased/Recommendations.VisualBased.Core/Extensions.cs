using Microsoft.Extensions.DependencyInjection;
using Recommendations.VisualBased.Core.ModuleApi;
using Recommendations.VisualBased.Shared;

namespace Recommendations.VisualBased.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IVisualBasedModuleApi, VisualBasedModuleApi>();

        return services;
    }
}