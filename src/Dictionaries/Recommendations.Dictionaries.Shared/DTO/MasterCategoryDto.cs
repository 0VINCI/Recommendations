namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record MasterCategoryDto(
    Guid Id,
    string Name,
    IReadOnlyCollection<SubCategoryDto> SubCategories); 