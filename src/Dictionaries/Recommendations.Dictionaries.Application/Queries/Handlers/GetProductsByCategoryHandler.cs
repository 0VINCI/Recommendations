using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetProductsByCategoryHandler(
    IProductRepository productRepository,
    ISubCategoryRepository subCategoryRepository,
    IMapper mapper
) : IQueryHandler<GetProductsByCategory, FilteredProductDto>
{
    public async Task<FilteredProductDto> HandleAsync(GetProductsByCategory query, CancellationToken cancellationToken = default)
    {
        var productsQuery = productRepository.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SubCategoryId))
        {
            productsQuery = productsQuery.Where(p => p.SubCategory.Id == Guid.Parse(query.SubCategoryId));
        }

        else if (!string.IsNullOrWhiteSpace(query.MasterCategoryId))
        {
            var subCategories = await subCategoryRepository.GetByMasterCategoryIdAsync(Guid.Parse(query.MasterCategoryId));
            var subCategoryIds = subCategories.Select(sc => sc.Id).ToList();
            productsQuery = productsQuery.Where(p => subCategoryIds.Contains(p.SubCategory.Id));
        }

        var totalCount = await productsQuery.CountAsync(cancellationToken);

        var products = await productsQuery
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Images)
            .OrderBy(p => p.ProductDisplayName)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var productDtos = mapper.Map<IReadOnlyCollection<ProductDto>>(products);
        var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new FilteredProductDto(productDtos, totalCount, query.Page, query.PageSize, totalPages);
    }
}
