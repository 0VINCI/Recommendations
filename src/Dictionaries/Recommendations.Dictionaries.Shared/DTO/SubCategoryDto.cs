namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record SubCategoryDto(
    Guid Id,
    string Name,
    Guid MasterCategoryId,
    IReadOnlyCollection<ArticleTypeDto> ArticleTypes); 