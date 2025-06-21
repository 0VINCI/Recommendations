using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Core.ModuleApi;
using Recommendations.Authorization.Shared;

namespace Recommendations.Authorization.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAuthorizationModuleApi, AuthorizationModuleApi>();

        return services;
    }
}