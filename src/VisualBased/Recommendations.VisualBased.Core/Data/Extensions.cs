using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recommendations.Shared.Infrastructure.Options;
using Recommendations.VisualBased.Core.Repositories;

namespace Recommendations.VisualBased.Core.Data;

internal static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        services.AddDbContext<VisualBasedDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>();
            options.UseNpgsql(dbOptions.Value.DatabaseConnection, npgsqlOptions =>
            {
                npgsqlOptions.UseVector();
            });
        });
        
        services.AddScoped<IVisualEmbeddingRepository, VisualEmbeddingRepository>();
        
        return services;
    }
}

