using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetProductsByIdsForRecommendationsHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetProductsByIdsForRecommendations, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(GetProductsByIdsForRecommendations query, CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetByIdsForRecommendationsAsync(query.ProductIds);
        return mapper.Map<IReadOnlyCollection<ProductDto>>(products);
    }
}
