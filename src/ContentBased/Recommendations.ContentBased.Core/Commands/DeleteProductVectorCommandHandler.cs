using Recommendations.ContentBased.Core.Repositories;
using Recommendations.ContentBased.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.ContentBased.Core.Commands;

internal sealed class DeleteProductEmbeddingCommandHandler(IProductEmbeddingRepository productEmbeddingRepository) 
    : ICommandHandler<DeleteProductEmbedding>
{
    public async Task HandleAsync(
        DeleteProductEmbedding command, 
        CancellationToken cancellationToken = default)
    {
        await productEmbeddingRepository.Delete(command.ProductId, command.Variant, cancellationToken);
    }
}

internal sealed class DeleteProductEmbeddingsCommandHandler(IProductEmbeddingRepository productEmbeddingRepository) 
    : ICommandHandler<DeleteProductEmbeddings>
{
    public async Task HandleAsync(
        DeleteProductEmbeddings command, 
        CancellationToken cancellationToken = default)
    {
        await productEmbeddingRepository.DeleteByProductId(command.ProductId, cancellationToken);
    }
}
