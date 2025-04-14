using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.Authorization.Infrastructure.DAL;

internal static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        services.AddDbContext<AuthorizationDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>();
            options.UseNpgsql(dbOptions.Value.DatabaseConnection);
        });

        return services;
    }
}