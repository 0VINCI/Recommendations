using Recommendations.ContentBased.Shared;
using Recommendations.ContentBased.Shared.Types;
using Recommendations.ContentBased.Shared.Commands;
using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Queries;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.ContentBased.Core.ModuleApi;

public class ContentBasedModuleApi(ICommandDispatcher commands,
    IQueryDispatcher queries) : IContentBasedModuleApi
{
    public async Task<ProductEmbeddingDto?> GetProductEmbedding(Guid productId, VectorType variant, CancellationToken cancellationToken = default)
    {
        var query = new GetProductEmbedding(productId, variant);
        return await queries.QueryAsync(query, cancellationToken);
    }

    public async Task<IEnumerable<ProductEmbeddingDto>> GetProductEmbeddings(Guid productId, CancellationToken cancellationToken = default)
    {
        var query = new GetProductEmbeddings(productId);
        return await queries.QueryAsync(query, cancellationToken);
    }

    public async Task<IEnumerable<SimilarProductDto>> GetSimilarProducts(
        Guid productId, 
        VectorType variant, 
        int topCount, 
        CancellationToken cancellationToken = default)
    {
        var query = new GetSimilarProducts(productId, variant, topCount);
        return await queries.QueryAsync(query, cancellationToken);
    }

    public async Task CreateProductEmbedding(CreateProductEmbeddingDto productEmbedding, CancellationToken cancellationToken = default)
    {
        var command = new CreateProductEmbedding(productEmbedding);
        await commands.SendAsync(command, cancellationToken);
    }

    public async Task UpdateProductEmbedding(Guid productId, VectorType variant, UpdateProductEmbeddingDto productEmbedding, CancellationToken cancellationToken = default)
    {
        var command = new UpdateProductEmbedding(productId, variant, productEmbedding);
        await commands.SendAsync(command, cancellationToken);
    }

    public async Task DeleteProductEmbedding(Guid productId, VectorType variant, CancellationToken cancellationToken = default)
    {
        var command = new DeleteProductEmbedding(productId, variant);
        await commands.SendAsync(command, cancellationToken);
    }

    public async Task DeleteProductEmbeddings(Guid productId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteProductEmbeddings(productId);
        await commands.SendAsync(command, cancellationToken);
    }
}