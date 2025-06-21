using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Infrastructure.DAL;

namespace Recommendations.Dictionaries.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPostgres();
        return services;
    }
}