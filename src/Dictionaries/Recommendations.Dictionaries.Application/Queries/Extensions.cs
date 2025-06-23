using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Application.Queries.Handlers;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetAllProducts, IReadOnlyCollection<ProductDto>>, GetAllProductsHandler>();
        services.AddScoped<IQueryHandler<GetProductById, ProductDto?>, GetProductByIdHandler>();
        services.AddScoped<IQueryHandler<GetProductsByCategory, IReadOnlyCollection<ProductDto>>, GetProductsByCategoryHandler>();
        services.AddScoped<IQueryHandler<GetBestsellers, IReadOnlyCollection<ProductDto>>, GetBestsellersHandler>();
        services.AddScoped<IQueryHandler<GetNewProducts, IReadOnlyCollection<ProductDto>>, GetNewProductsHandler>();
        services.AddScoped<IQueryHandler<SearchProducts, IReadOnlyCollection<ProductDto>>, SearchProductsHandler>();
        services.AddScoped<IQueryHandler<GetProductFullById, ProductFullDto?>, GetProductFullByIdHandler>();
        
        return services;
    }
}