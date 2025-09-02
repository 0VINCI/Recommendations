using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Core.Services;
using Recommendations.Dictionaries.Infrastructure.DAL.Repositories;
using Recommendations.Dictionaries.Infrastructure.Services.ImportDataset.FashionDataset;
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
        services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
        services.AddScoped<IMasterCategoryRepository, MasterCategoryRepository>();
        services.AddScoped<IArticleTypeRepository, ArticleTypeRepository>();
        services.AddScoped<IBaseColourRepository, BaseColourRepository>();
        services.AddScoped<IDataImportService, DataImportService>();
        services.AddScoped<IEmbeddingsImportService, EmbeddingsImportService>();

        return services;
    }
}