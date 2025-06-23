using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetProductsByCategoryHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetProductsByCategory, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(GetProductsByCategory query, CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetByCategoryAsync(query.Category);
        return mapper.Map<IReadOnlyCollection<ProductDto>>(products);
    }
} 