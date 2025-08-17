using Recommendations.ContentBased.Shared.Commands;
using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Queries;
using Recommendations.ContentBased.Shared.Types;

namespace Recommendations.ContentBased.Shared;

public interface IContentBasedModuleApi
{
    Task<ProductEmbeddingDto?> GetProductEmbedding(Guid productId, VectorType variant, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductEmbeddingDto>> GetProductEmbeddings(Guid productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SimilarProductDto>> GetSimilarProducts(Guid productId, VectorType variant, int topCount, CancellationToken cancellationToken = default);
    Task CreateProductEmbedding(CreateProductEmbeddingDto productEmbedding, CancellationToken cancellationToken = default);
    Task UpdateProductEmbedding(Guid productId, VectorType variant, UpdateProductEmbeddingDto productEmbedding, CancellationToken cancellationToken = default);
    Task DeleteProductEmbedding(Guid productId, VectorType variant, CancellationToken cancellationToken = default);
    Task DeleteProductEmbeddings(Guid productId, CancellationToken cancellationToken = default);
}