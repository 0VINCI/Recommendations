using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recommendations.Authorization.Infrastructure.DAL;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.Authorization.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPostgres();
        return services;
    }
}