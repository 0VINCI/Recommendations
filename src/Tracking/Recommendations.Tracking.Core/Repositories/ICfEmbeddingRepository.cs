namespace Recommendations.Tracking.Core.Repositories;

/// <summary>
/// Repository for Collaborative Filtering embeddings.
/// Provides similarity search using pgvector cosine distance.
/// </summary>
public interface ICfEmbeddingRepository
{
    /// <summary>
    /// Get similar items based on CF embeddings (item-to-item similarity).
    /// </summary>
    Task<IEnumerable<(string ItemId, float SimilarityScore)>> GetSimilarItems(
        string itemId,
        int topCount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get recommended items for a user based on CF embeddings (user-to-item similarity).
    /// </summary>
    Task<IEnumerable<(string ItemId, float SimilarityScore)>> GetRecommendationsForUser(
        string userKey,
        int topCount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if user embedding exists.
    /// </summary>
    Task<bool> UserEmbeddingExists(string userKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if item embedding exists.
    /// </summary>
    Task<bool> ItemEmbeddingExists(string itemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get count of user embeddings.
    /// </summary>
    Task<int> GetUserEmbeddingsCount(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get count of item embeddings.
    /// </summary>
    Task<int> GetItemEmbeddingsCount(CancellationToken cancellationToken = default);
}

