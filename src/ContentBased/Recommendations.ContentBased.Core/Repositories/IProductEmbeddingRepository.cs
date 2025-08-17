using Recommendations.ContentBased.Core.Types;
using Recommendations.ContentBased.Shared.Types;

namespace Recommendations.ContentBased.Core.Repositories;

public interface IProductEmbeddingRepository
{
    Task<ProductEmbedding?> GetByProductIdAndVariant(Guid productId, VectorType variant, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductEmbedding>> GetByProductId(Guid productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductEmbedding>> GetAll(CancellationToken cancellationToken = default);
    Task<IEnumerable<(ProductEmbedding ProductEmbedding, float SimilarityScore)>> GetSimilarProducts(Guid productId, VectorType variant, int topCount, CancellationToken cancellationToken = default);
    Task Save(ProductEmbedding productEmbedding, CancellationToken cancellationToken = default);
    Task SaveRange(IEnumerable<ProductEmbedding> productEmbeddings, CancellationToken cancellationToken = default);
    Task Delete(Guid productId, VectorType variant, CancellationToken cancellationToken = default);
    Task DeleteByProductId(Guid productId, CancellationToken cancellationToken = default);
}
