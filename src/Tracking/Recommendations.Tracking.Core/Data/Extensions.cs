using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Pgvector.EntityFrameworkCore;
using Recommendations.Shared.Infrastructure.Options;
using Recommendations.Tracking.Core.Data.Repositories;
using Recommendations.Tracking.Core.Data.Signals;
using Recommendations.Tracking.Core.Data.Tracking;
using Recommendations.Tracking.Core.Repositories;
using Recommendations.Tracking.Core.Types;

namespace Recommendations.Tracking.Core.Data;

internal static class Extensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services)
    {
        services.AddSingleton<NpgsqlDataSource>(serviceProvider =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>();
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(dbOptions.Value.DatabaseConnection);
            dataSourceBuilder.MapEnum<EventSource>("rec.event_source");
            return dataSourceBuilder.Build();
        });
        
        services.AddDbContext<TrackingDbContext>((serviceProvider, options) =>
        {
            var dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();
            options.UseNpgsql(dataSource);
        });
        
        services.AddDbContext<SignalsDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<DbOptions>>();
            options.UseNpgsql(dbOptions.Value.DatabaseConnection, npgsqlOptions =>
            {
                npgsqlOptions.UseVector();
            });
        });
        
        services.AddScoped<ITrackingRepository, TrackingRepository>();
        services.AddScoped<ICfEmbeddingRepository, CfEmbeddingRepository>();

        return services;
    }
}
