using Recommendations.ContentBased.Core.Repositories;
using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.ContentBased.Core.Queries;

internal sealed class GetSimilarProductsQueryHandler(IProductEmbeddingRepository productEmbeddingRepository) 
    : IQueryHandler<GetSimilarProducts, IEnumerable<SimilarProductDto>>
{
    public async Task<IEnumerable<SimilarProductDto>> HandleAsync(
        GetSimilarProducts query, 
        CancellationToken cancellationToken = default)
    {
        var similarProducts = await productEmbeddingRepository.GetSimilarProducts(
            query.ProductId, 
            query.Variant, 
            query.TopCount, 
            cancellationToken);

        return similarProducts.Select(result => 
            new SimilarProductDto(result.ProductEmbedding.ProductId, result.SimilarityScore));
    }
}
