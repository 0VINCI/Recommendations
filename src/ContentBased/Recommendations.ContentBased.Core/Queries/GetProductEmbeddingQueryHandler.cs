using Recommendations.ContentBased.Core.Repositories;
using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.ContentBased.Core.Queries;

internal sealed class GetProductEmbeddingQueryHandler(IProductEmbeddingRepository productEmbeddingRepository) 
    : IQueryHandler<GetProductEmbedding, ProductEmbeddingDto?>
{
    public async Task<ProductEmbeddingDto?> HandleAsync(
        GetProductEmbedding query, 
        CancellationToken cancellationToken = default)
    {
        var productEmbedding = await productEmbeddingRepository.GetByProductIdAndVariant(
            query.ProductId, query.Variant, cancellationToken);
        
        if (productEmbedding == null)
            return null;

        return new ProductEmbeddingDto(
            productEmbedding.ProductId,
            productEmbedding.Variant,
            productEmbedding.Embedding,
            productEmbedding.CreatedAt,
            productEmbedding.UpdatedAt);
    }
}
