using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Application.Commands;
using Recommendations.Authorization.Application.Queries;

namespace Recommendations.Authorization.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommands();
        services.AddQueries();
        services.AddAutoMapper(typeof(AuthorizationMappingProfile).Assembly);

        return services;
    }
}