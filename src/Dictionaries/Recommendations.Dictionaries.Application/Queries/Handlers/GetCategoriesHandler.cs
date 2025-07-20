using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetCategoriesHandler(
    IMasterCategoryRepository masterCategoryRepository,
    IMapper mapper) : IQueryHandler<GetCategories, IReadOnlyCollection<CategoriesDto>>
{
    public async Task<IReadOnlyCollection<CategoriesDto>> HandleAsync(GetCategories query, CancellationToken cancellationToken = default)
    {
        var masterCategories = await masterCategoryRepository.GetAllAsync();
        return mapper.Map<IReadOnlyCollection<CategoriesDto>>(masterCategories ?? []);
    }
}