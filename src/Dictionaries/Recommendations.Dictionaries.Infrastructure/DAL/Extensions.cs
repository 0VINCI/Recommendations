using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Infrastructure.DAL.Repositories;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.Dictionaries.Infrastructure.DAL;

internal static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        services.AddDbContext<DictionariesDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>();
            options.UseNpgsql(dbOptions.Value.DatabaseConnection);
        });
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}