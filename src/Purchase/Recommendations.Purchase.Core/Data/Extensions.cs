using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.Purchase.Core.Data;

internal static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        services.AddDbContext<PurchaseDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>();
            options.UseNpgsql(dbOptions.Value.DatabaseConnection);
        });
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        services.AddAutoMapper(typeof(PurchaseMappingProfile).Assembly);

        return services;
    }
}