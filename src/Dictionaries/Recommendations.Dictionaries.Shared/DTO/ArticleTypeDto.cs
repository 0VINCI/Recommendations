namespace Recommendations.Dictionaries.Shared.DTO;

public sealed record ArticleTypeDto(
    Guid Id,
    string Name,
    Guid SubCategoryId); 