using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetNewProductsHandler(IProductRepository productRepository) : IQueryHandler<GetNewProducts, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(GetNewProducts query, CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetNewProductsAsync();
        
        return products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Price,
            p.OriginalPrice,
            p.Image,
            p.Category,
            p.Description,
            p.Sizes,
            p.Colors,
            p.Rating,
            p.Reviews,
            p.IsBestseller,
            p.IsNew)).ToList();
    }
} 