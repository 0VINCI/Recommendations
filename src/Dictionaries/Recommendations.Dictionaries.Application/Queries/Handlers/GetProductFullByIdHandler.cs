using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetProductFullByIdHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetProductFullById, ProductFullDto?>
{
    public async Task<ProductFullDto?> HandleAsync(GetProductFullById query, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdWithDetailsAsync(query.Id);
        return product != null ? mapper.Map<ProductFullDto>(product) : null;
    }
}