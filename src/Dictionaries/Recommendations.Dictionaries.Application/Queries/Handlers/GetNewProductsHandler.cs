using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetNewProductsHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetNewProducts, FilteredProductDto>
{
    public async Task<FilteredProductDto> HandleAsync(GetNewProducts query, CancellationToken cancellationToken = default)
    {
        var result = await productRepository.GetFilteredAsync(
            subCategoryId: null,
            masterCategoryId: null,
            articleTypeId: null,
            baseColourId: null,
            minPrice: null,
            maxPrice: null,
            isBestseller: null,
            isNew: true,
            searchTerm: query.SearchTerm,
            page: query.Page,
            pageSize: query.PageSize,
            cancellationToken
        );

        var productDtos = mapper.Map<IReadOnlyCollection<ProductDto>>(result.Products);
        var totalPages = (int)Math.Ceiling((double)result.TotalCount / query.PageSize);

        return new FilteredProductDto(productDtos, result.TotalCount, query.Page, query.PageSize, totalPages);
    }
}