using Microsoft.Extensions.DependencyInjection;
using Recommendations.Cart.Application.Commands;
using Recommendations.Cart.Application.Queries;

namespace Recommendations.Cart.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommands();
        services.AddQueries();
        services.AddAutoMapper(typeof(CartMappingProfile).Assembly);

        return services;
    }
}