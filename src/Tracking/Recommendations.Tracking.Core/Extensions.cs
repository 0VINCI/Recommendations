using Microsoft.Extensions.DependencyInjection;
using Recommendations.Tracking.Core.ModuleApi;
using Recommendations.Tracking.Shared;

namespace Recommendations.Tracking.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<ITrackingModuleApi, TrackingModuleApi>();

        return services;
    }
}