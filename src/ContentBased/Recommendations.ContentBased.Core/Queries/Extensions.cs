using Microsoft.Extensions.DependencyInjection;
using Recommendations.ContentBased.Core.Queries;
using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.ContentBased.Core.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetProductEmbedding, ProductEmbeddingDto?>, GetProductEmbeddingQueryHandler>();
        services.AddScoped<IQueryHandler<GetProductEmbeddings, IEnumerable<ProductEmbeddingDto>>, GetProductEmbeddingsQueryHandler>();
        services.AddScoped<IQueryHandler<GetSimilarProducts, IEnumerable<SimilarProductDto>>, GetSimilarProductsQueryHandler>();
        
        return services;
    }
}