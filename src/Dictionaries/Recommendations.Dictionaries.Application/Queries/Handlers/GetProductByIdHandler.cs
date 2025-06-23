using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetProductByIdHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetProductById, ProductDto?>
{
    public async Task<ProductDto?> HandleAsync(GetProductById query, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(query.Id);
        return product != null ? mapper.Map<ProductDto>(product) : null;
    }
} 