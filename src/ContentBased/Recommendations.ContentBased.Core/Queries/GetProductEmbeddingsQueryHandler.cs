using Recommendations.ContentBased.Core.Repositories;
using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.ContentBased.Core.Queries;

internal sealed class GetProductEmbeddingsQueryHandler(IProductEmbeddingRepository productEmbeddingRepository) 
    : IQueryHandler<GetProductEmbeddings, IEnumerable<ProductEmbeddingDto>>
{
    public async Task<IEnumerable<ProductEmbeddingDto>> HandleAsync(
        GetProductEmbeddings query, 
        CancellationToken cancellationToken = default)
    {
        var productEmbeddings = await productEmbeddingRepository.GetByProductId(query.ProductId, cancellationToken);
        
        return productEmbeddings.Select(pe => new ProductEmbeddingDto(
            pe.ProductId,
            pe.Variant,
            pe.Embedding,
            pe.CreatedAt,
            pe.UpdatedAt));
    }
}
