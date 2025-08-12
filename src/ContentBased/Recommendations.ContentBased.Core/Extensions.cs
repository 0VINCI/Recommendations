using Microsoft.Extensions.DependencyInjection;
using Recommendations.ContentBased.Core.Commands;
using Recommendations.ContentBased.Core.ModuleApi;
using Recommendations.ContentBased.Core.Queries;
using Recommendations.ContentBased.Shared;

namespace Recommendations.ContentBased.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IContentBasedModuleApi, ContentBasedModuleApi>();
        services.AddCommands();
        services.AddQueries();
        //services.AddPostgres();

        return services;
    }
}