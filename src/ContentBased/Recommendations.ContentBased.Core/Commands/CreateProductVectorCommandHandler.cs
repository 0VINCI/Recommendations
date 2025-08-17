using Recommendations.ContentBased.Core.Repositories;
using Recommendations.ContentBased.Core.Types;
using Recommendations.ContentBased.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.ContentBased.Core.Commands;

internal sealed class CreateProductEmbeddingCommandHandler(IProductEmbeddingRepository productEmbeddingRepository) 
    : ICommandHandler<CreateProductEmbedding>
{
    public async Task HandleAsync(
        CreateProductEmbedding command, 
        CancellationToken cancellationToken = default)
    {
        var productEmbedding = ProductEmbedding.Create(
            command.ProductEmbedding.ProductId,
            command.ProductEmbedding.Variant,
            command.ProductEmbedding.Embedding);

        await productEmbeddingRepository.Save(productEmbedding, cancellationToken);
    }
}
