using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetBestsellersHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetBestsellers, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(GetBestsellers query, CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetBestsellersAsync();
        return mapper.Map<IReadOnlyCollection<ProductDto>>(products);
    }
} 