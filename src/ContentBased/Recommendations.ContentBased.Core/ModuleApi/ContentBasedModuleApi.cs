using Microsoft.Extensions.Caching.Memory;
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
    IDictionariesModuleApi dictionariesModuleApi,
    IMemoryCache memoryCache) : IContentBasedModuleApi
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
        bool useNew = true, 
        CancellationToken cancellationToken = default)
    {
        // Cache key dla rekomendacji
        var cacheKey = $"similar_products_{productId}_{variant}_{topCount}_{(useNew ? "new" : "old")}";
        
        // Sprawdź cache
        if (memoryCache.TryGetValue(cacheKey, out IEnumerable<ProductDto>? cachedProducts))
        {
            return cachedProducts!;
        }

        // 1. Pobierz podobne produkty (ID + similarity score)
        var query = new GetSimilarProducts(productId, variant, topCount, useNew);
        var similarProducts = await queries.QueryAsync(query, cancellationToken);
        
        // 2. Pobierz zoptymalizowane dane produktów (tylko niezbędne pola)
        var productIds = similarProducts.Select(sp => sp.ProductId).ToArray();
        var products = await dictionariesModuleApi.GetProductsByIdsForRecommendations(productIds);
        
        // 3. Cache na 5 minut
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };
        memoryCache.Set(cacheKey, products, cacheOptions);
        
        // 4. Zwróć tylko produkty (bez similarity score)
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