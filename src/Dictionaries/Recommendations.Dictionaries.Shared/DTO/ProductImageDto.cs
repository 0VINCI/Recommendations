namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record ProductImageDto(
    Guid Id,
    Guid ProductId,
    string ImageUrl,
    string ImageType,
    string? Resolution,
    bool IsPrimary); 