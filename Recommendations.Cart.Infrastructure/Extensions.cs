using Microsoft.Extensions.DependencyInjection;
using Recommendations.Cart.Infrastructure.DAL;

namespace Recommendations.Cart.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPostgres();
        return services;
    }
}