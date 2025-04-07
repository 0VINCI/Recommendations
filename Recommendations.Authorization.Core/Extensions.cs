using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Recommendations.Authorization.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}