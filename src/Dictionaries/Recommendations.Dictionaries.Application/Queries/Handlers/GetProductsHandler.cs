using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetProductsHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetProducts, FilteredProductDto>
{
    public async Task<FilteredProductDto> HandleAsync(GetProducts query, CancellationToken cancellationToken = default)
    {
        var productsQuery = productRepository.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SubCategoryId))
            productsQuery = productsQuery.Where(p => p.SubCategory.Id == Guid.Parse(query.SubCategoryId));

        if (!string.IsNullOrWhiteSpace(query.MasterCategoryId))
            productsQuery = productsQuery.Where(p => p.SubCategory.MasterCategoryId == Guid.Parse(query.MasterCategoryId));
        
        if (!string.IsNullOrWhiteSpace(query.ArticleTypeId))
            productsQuery = productsQuery.Where(p => p.ArticleType.Id == Guid.Parse(query.ArticleTypeId));
        
        if (!string.IsNullOrWhiteSpace(query.BaseColourId))
            productsQuery = productsQuery.Where(p => p.BaseColour.Id == Guid.Parse(query.BaseColourId));
        
        if (query.MinPrice.HasValue)
            productsQuery = productsQuery.Where(p => p.Price >= query.MinPrice.Value);
        
        if (query.MaxPrice.HasValue)
            productsQuery = productsQuery.Where(p => p.Price <= query.MaxPrice.Value);
        
        if (query.IsBestseller.HasValue)
            productsQuery = productsQuery.Where(p => p.IsBestseller == query.IsBestseller.Value);
        
        if (query.IsNew.HasValue)
            productsQuery = productsQuery.Where(p => p.IsNew == query.IsNew.Value);
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            productsQuery = productsQuery.Where(p =>
                p.ProductDisplayName.Contains(query.SearchTerm) ||
                p.SubCategory.Name.Contains(query.SearchTerm) ||
                p.ArticleType.Name.Contains(query.SearchTerm));

        var totalCount = await productsQuery.CountAsync(cancellationToken);

        var products = await productsQuery
            .OrderBy(p => p.ProductDisplayName) 
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var productDtos = mapper.Map<IReadOnlyCollection<ProductDto>>(products);
        var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new FilteredProductDto(productDtos, totalCount, query.Page, query.PageSize, totalPages);
    }
}
