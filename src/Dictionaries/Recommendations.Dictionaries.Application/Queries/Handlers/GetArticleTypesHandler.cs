using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetArticleTypesHandler(
    IArticleTypeRepository articleTypeRepository) : IQueryHandler<GetArticleTypes, IReadOnlyCollection<ArticleTypeDto>>
{
    public async Task<IReadOnlyCollection<ArticleTypeDto>> HandleAsync(GetArticleTypes query, CancellationToken cancellationToken = default)
    {
        var allArticleTypes = await articleTypeRepository.GetAllAsync();
        
        var filteredArticleTypes = query.SubCategoryId.HasValue
            ? allArticleTypes.Where(at => at.SubCategoryId == query.SubCategoryId.Value)
            : allArticleTypes;
        
        var result = filteredArticleTypes.Select(at => new ArticleTypeDto(
            at.Id,
            at.Name,
            at.SubCategoryId
        )).ToList();
        
        return result;
    }
}
