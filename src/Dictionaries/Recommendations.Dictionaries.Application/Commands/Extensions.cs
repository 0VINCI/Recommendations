using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Application.Commands.Handlers;
using Recommendations.Dictionaries.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Dictionaries.Application.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<AddProduct>, AddProductHandler>();
        services.AddScoped<ICommandHandler<UpdateProduct>, UpdateProductHandler>();
        services.AddScoped<ICommandHandler<DeleteProduct>, DeleteProductHandler>();

        return services;
    }
}