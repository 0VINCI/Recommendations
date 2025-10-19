using Microsoft.Extensions.DependencyInjection;
using Recommendations.Tracking.Core.Data;
using Recommendations.Tracking.Core.Events;
using Recommendations.Tracking.Core.ModuleApi;
using Recommendations.Tracking.Shared;

namespace Recommendations.Tracking.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<ITrackingModuleApi, TrackingModuleApi>();
        services.AddPostgres();
        services.AddEvents();

        return services;
    }
}