using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recommendations.ContentBased.Core.Data.Repositories;
using Recommendations.ContentBased.Core.Repositories;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.ContentBased.Core.Data;

internal static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        services.AddDbContext<ContentBasedDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>();
            options.UseNpgsql(dbOptions.Value.DatabaseConnection, npgsqlOptions =>
            {
                npgsqlOptions.UseVector();
            });
        });
        
        services.AddScoped<IProductEmbeddingRepository, ProductEmbeddingRepository>();
        
        return services;
    }
}
