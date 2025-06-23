using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Application.Commands;
using Recommendations.Dictionaries.Application.Queries;

namespace Recommendations.Dictionaries.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommands();
        services.AddQueries();
        services.AddAutoMapper(typeof(DictionariesMappingProfile).Assembly);

        return services;
    }
}