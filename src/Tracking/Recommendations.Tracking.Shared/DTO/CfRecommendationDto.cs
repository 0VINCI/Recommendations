namespace Recommendations.Tracking.Shared.DTO;

/// <summary>
/// DTO for CF recommendation result.
/// </summary>
public record CfRecommendationDto(string ItemId, float SimilarityScore);

/// <summary>
/// Response containing CF recommendations.
/// </summary>
public record CfRecommendationsResponse(
    IEnumerable<CfRecommendationDto> Items,
    int Count,
    string Source);

/// <summary>
/// CF model statistics.
/// </summary>
public record CfModelStatsResponse(
    int UserEmbeddingsCount,
    int ItemEmbeddingsCount);

