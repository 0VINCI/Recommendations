namespace Recommendations.VisualBased.Core.Repositories;

public interface IVisualEmbeddingRepository
{
    Task<IEnumerable<(string ItemId, float SimilarityScore)>> GetSimilarItems(string itemId, int topCount, CancellationToken cancellationToken = default);
    Task<bool> ItemEmbeddingExists(string itemId, CancellationToken cancellationToken = default);
    Task<long> GetItemEmbeddingsCount(CancellationToken cancellationToken = default);
}

