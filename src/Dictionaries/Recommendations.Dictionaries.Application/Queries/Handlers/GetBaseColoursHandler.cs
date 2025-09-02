using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetBaseColoursHandler(
    IBaseColourRepository baseColourRepository) : IQueryHandler<GetBaseColours, IReadOnlyCollection<BaseColourDto>>
{
    public async Task<IReadOnlyCollection<BaseColourDto>> HandleAsync(GetBaseColours query, CancellationToken cancellationToken = default)
    {
        var allBaseColours = await baseColourRepository.GetAllAsync();
        
        var result = allBaseColours.Select(bc => new BaseColourDto(
            bc.Id,
            bc.Name
        )).ToList();
        
        return result;
    }
}
