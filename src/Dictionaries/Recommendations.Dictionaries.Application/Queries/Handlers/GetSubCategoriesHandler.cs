using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetSubCategoriesHandler(
    ISubCategoryRepository subCategoryRepository,
    IArticleTypeRepository articleTypeRepository) : IQueryHandler<GetSubCategories, IReadOnlyCollection<SubCategoryDto>>
{
    public async Task<IReadOnlyCollection<SubCategoryDto>> HandleAsync(GetSubCategories query, CancellationToken cancellationToken = default)
    {
        var allSubCategories = await subCategoryRepository.GetAllAsync();
        var allArticleTypes = await articleTypeRepository.GetAllAsync();
        
        var filteredSubCategories = query.MasterCategoryId.HasValue
            ? allSubCategories.Where(sc => sc.MasterCategoryId == query.MasterCategoryId.Value)
            : allSubCategories;
        
        var result = filteredSubCategories.Select(sc => new SubCategoryDto(
            sc.Id,
            sc.Name,
            sc.MasterCategoryId,
            allArticleTypes
                .Where(at => at.SubCategoryId == sc.Id)
                .Select(at => new ArticleTypeDto(
                    at.Id,
                    at.Name,
                    at.SubCategoryId
                )).ToList()
        )).ToList();
        
        return result;
    }
}
