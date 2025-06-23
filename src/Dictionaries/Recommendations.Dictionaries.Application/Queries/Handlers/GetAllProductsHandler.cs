using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetAllProductsHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetAllProducts, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(GetAllProducts query, CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetAllAsync();
        return mapper.Map<IReadOnlyCollection<ProductDto>>(products);
    }
}