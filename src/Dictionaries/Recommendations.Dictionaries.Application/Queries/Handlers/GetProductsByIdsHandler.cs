using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetProductsByIdsHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetProductsByIds, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(GetProductsByIds query, CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetByIdsAsync(query.ProductIds);
        return mapper.Map<IReadOnlyCollection<ProductDto>>(products);
    }
}
