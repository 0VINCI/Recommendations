using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetCategoriesHandler(
    IMasterCategoryRepository masterCategoryRepository,
    ISubCategoryRepository subCategoryRepository,
    IArticleTypeRepository articleTypeRepository) : IQueryHandler<GetCategories, IReadOnlyCollection<MasterCategoryDto>>
{
    public async Task<IReadOnlyCollection<MasterCategoryDto>> HandleAsync(GetCategories query, CancellationToken cancellationToken = default)
    {
        var masterCategories = await masterCategoryRepository.GetAllAsync();
        var allSubCategories = await subCategoryRepository.GetAllAsync();
        var allArticleTypes = await articleTypeRepository.GetAllAsync();
        
        var result = masterCategories?.Select(mc => new MasterCategoryDto(
            mc.Id,
            mc.Name,
            allSubCategories
                .Where(sc => sc.MasterCategoryId == mc.Id)
                .Select(sc => new SubCategoryDto(
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
                )).ToList()
        )).ToList() ?? new List<MasterCategoryDto>();
        
        return result;
    }
}