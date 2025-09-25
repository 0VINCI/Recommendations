using Recommendations.ContentBased.Shared;
using Recommendations.ContentBased.Shared.Types;
using Recommendations.ContentBased.Shared.Commands;
using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Queries;
using Recommendations.Dictionaries.Shared;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.ContentBased.Core.ModuleApi;

public class ContentBasedModuleApi(ICommandDispatcher commands,
    IQueryDispatcher queries,
    IDictionariesModuleApi dictionariesModuleApi) : IContentBasedModuleApi
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

    public async Task<IEnumerable<ProductDto>> GetSimilarProducts(
        Guid productId, 
        VectorType variant, 
        int topCount, 
        CancellationToken cancellationToken = default)
    {
        // 1. Pobierz podobne produkty (ID + similarity score)
        var query = new GetSimilarProducts(productId, variant, topCount);
        var similarProducts = await queries.QueryAsync(query, cancellationToken);
        
        // 2. Pobierz pełne dane produktów
        var productIds = similarProducts.Select(sp => sp.ProductId).ToArray();
        var products = await dictionariesModuleApi.GetProductsByIds(productIds);
        
        // 3. Zwróć tylko produkty (bez similarity score)
        return products;
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