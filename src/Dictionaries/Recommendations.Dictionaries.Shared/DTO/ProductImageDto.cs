namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record ProductImageDto(
    string ImageUrl,
    string ImageType,
    string? Resolution,
    bool IsPrimary); 