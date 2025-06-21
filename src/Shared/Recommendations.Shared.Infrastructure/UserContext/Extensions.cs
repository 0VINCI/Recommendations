using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Shared.Infrastructure.UserContext;

public static class Extensions
{
    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
        
        return services;
    }
}