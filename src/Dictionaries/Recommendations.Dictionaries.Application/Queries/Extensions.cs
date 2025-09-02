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
        services.AddScoped<IQueryHandler<GetProductsByCategory, FilteredProductDto>, GetProductsByCategoryHandler>();
        services.AddScoped<IQueryHandler<GetBestsellers, FilteredProductDto>, GetBestsellersHandler>();
        services.AddScoped<IQueryHandler<GetNewProducts, FilteredProductDto>, GetNewProductsHandler>();
        services.AddScoped<IQueryHandler<SearchProducts, IReadOnlyCollection<ProductDto>>, SearchProductsHandler>();
        services.AddScoped<IQueryHandler<GetProductFullById, ProductFullDto?>, GetProductFullByIdHandler>();
        services.AddScoped<IQueryHandler<GetProducts, FilteredProductDto>, GetProductsHandler>();
        services.AddScoped<IQueryHandler<GetCategories, IReadOnlyCollection<MasterCategoryDto>>, GetCategoriesHandler>();
        services.AddScoped<IQueryHandler<GetSubCategories, IReadOnlyCollection<SubCategoryDto>>, GetSubCategoriesHandler>();
        services.AddScoped<IQueryHandler<GetArticleTypes, IReadOnlyCollection<ArticleTypeDto>>, GetArticleTypesHandler>();
        services.AddScoped<IQueryHandler<GetBaseColours, IReadOnlyCollection<BaseColourDto>>, GetBaseColoursHandler>();
        
        return services;
    }
}