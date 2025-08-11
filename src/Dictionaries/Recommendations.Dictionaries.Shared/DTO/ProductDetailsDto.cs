namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record ProductDetailsDto(
    Guid Id,
    Guid ProductId,
    string Gender,
    string Season,
    string Usage,
    string? Year,
    string? Description,
    string? SleeveLength,
    string? Fit,
    string? Fabric,
    string? Collar,
    string? BodyOrGarmentSize,
    string? Pattern,
    string? AgeGroup);