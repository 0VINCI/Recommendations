using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetBestsellersHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetBestsellers, FilteredProductDto>
{
    public async Task<FilteredProductDto> HandleAsync(GetBestsellers query, CancellationToken cancellationToken = default)
    {
        var page = query.Page > 0 ? query.Page : 1;
        var pageSize = query.PageSize > 0 ? query.PageSize : 20;

        var productsQuery = productRepository.AsQueryable()
            .Where(p => p.IsBestseller);

        var totalCount = await productsQuery.CountAsync(cancellationToken);

        var products = await productRepository.GetBestsellersAsync();
        var productDtos = mapper.Map<IReadOnlyCollection<ProductDto>>(products);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new FilteredProductDto(productDtos, totalCount, page, pageSize, totalPages);
    }
} 