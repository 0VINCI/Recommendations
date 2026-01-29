using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetTrendingProductsHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetTrendingProducts, FilteredProductDto>
{
    public async Task<FilteredProductDto> HandleAsync(GetTrendingProducts query, CancellationToken cancellationToken = default)
    {
        var result = await productRepository.GetTrendingProductsPagedAsync(
            page: query.Page,
            pageSize: query.PageSize,
            cancellationToken
        );

        var productDtos = mapper.Map<IReadOnlyCollection<ProductDto>>(result.Products);
        var totalPages = (int)Math.Ceiling((double)result.TotalCount / query.PageSize);

        return new FilteredProductDto(productDtos, result.TotalCount, query.Page, query.PageSize, totalPages);
    }
}

