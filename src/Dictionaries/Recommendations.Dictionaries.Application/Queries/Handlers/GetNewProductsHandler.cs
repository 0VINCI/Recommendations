using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetNewProductsHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetNewProducts, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(GetNewProducts query, CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetNewProductsAsync();
        return mapper.Map<IReadOnlyCollection<ProductDto>>(products);
    }
} 