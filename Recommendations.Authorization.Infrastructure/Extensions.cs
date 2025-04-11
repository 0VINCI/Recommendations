using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Infrastructure.EF;

namespace Recommendations.Authorization.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthorizationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DatabaseConnection")));
        return services;
    }
}