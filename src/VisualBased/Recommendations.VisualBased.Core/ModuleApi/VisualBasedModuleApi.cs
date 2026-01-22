using Recommendations.Dictionaries.Shared;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.VisualBased.Core.Repositories;
using Recommendations.VisualBased.Shared;

namespace Recommendations.VisualBased.Core.ModuleApi;

public sealed class VisualBasedModuleApi(
    IVisualEmbeddingRepository visualEmbeddingRepository,
    IDictionariesModuleApi dictionariesModuleApi) : IVisualBasedModuleApi
{
    public async Task<IEnumerable<ProductDto>> GetSimilarProducts(
        Guid productId,
        int topCount,
        CancellationToken cancellationToken = default)
    {
        var productIdString = productId.ToString();
        
        var similarItems = await visualEmbeddingRepository.GetSimilarItems(productIdString, topCount, cancellationToken);
        
        if (!similarItems.Any())
            return [];
        
        var itemIds = similarItems.Select(si => Guid.Parse(si.ItemId)).ToArray();
        var products = await dictionariesModuleApi.GetProductsByIdsForRecommendations(itemIds);
        
        return products;
    }
}