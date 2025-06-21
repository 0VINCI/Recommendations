using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Core.ModuleApi;
using Recommendations.Dictionaries.Shared;

namespace Recommendations.Dictionaries.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IDictionariesModuleApi, DictionariesModuleApi>();

        return services;
    }
}