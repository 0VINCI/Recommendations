using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Infrastructure.DAL;

namespace Recommendations.Authorization.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPostgres();
        return services;
    }
}