using Microsoft.Extensions.DependencyInjection;
using Recommendations.ContentBased.Core.Commands;
using Recommendations.ContentBased.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.ContentBased.Core.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateProductEmbedding>, CreateProductEmbeddingCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateProductEmbedding>, UpdateProductEmbeddingCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteProductEmbedding>, DeleteProductEmbeddingCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteProductEmbeddings>, DeleteProductEmbeddingsCommandHandler>();
        
        return services;
    }
}