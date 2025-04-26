using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recommendations.Cart.Infrastructure.DAL.Repositories;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.Cart.Infrastructure.DAL;

internal static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        services.AddDbContext<CartDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>();
            options.UseNpgsql(dbOptions.Value.DatabaseConnection);
        });
        services.AddScoped<ICartRepository, CartRepository>();

        return services;
    }
}