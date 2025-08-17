using Recommendations.ContentBased.Core.Repositories;
using Recommendations.ContentBased.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.ContentBased.Core.Commands;

internal sealed class UpdateProductEmbeddingCommandHandler(IProductEmbeddingRepository productEmbeddingRepository) 
    : ICommandHandler<UpdateProductEmbedding>
{
    public async Task HandleAsync(
        UpdateProductEmbedding command, 
        CancellationToken cancellationToken = default)
    {
        var productEmbedding = await productEmbeddingRepository.GetByProductIdAndVariant(
            command.ProductId, command.Variant, cancellationToken);
        
        if (productEmbedding == null)
            throw new InvalidOperationException($"ProductEmbedding with ProductId {command.ProductId} and Variant {command.Variant} not found");

        productEmbedding.Update(command.ProductEmbedding.Embedding);

        await productEmbeddingRepository.Save(productEmbedding, cancellationToken);
    }
}
