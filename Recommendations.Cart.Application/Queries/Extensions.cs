using Microsoft.Extensions.DependencyInjection;
using Recommendations.Cart.Application.Queries.Handlers;
using Recommendations.Cart.Shared.DTO;
using Recommendations.Cart.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Cart.Application.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetCart, ShoppingCartDto?>, GetCartHandler>();
        
        return services;
    }
}