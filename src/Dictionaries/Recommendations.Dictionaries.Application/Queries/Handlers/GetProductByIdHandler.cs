using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class GetProductByIdHandler(IProductRepository productRepository) : IQueryHandler<GetProductById, ProductDto?>
{
    public async Task<ProductDto?> HandleAsync(GetProductById query, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(query.Id);
        
        if (product == null)
            return null;

        return new ProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.OriginalPrice,
            product.Image,
            product.Category,
            product.Description,
            product.Sizes,
            product.Colors,
            product.Rating,
            product.Reviews,
            product.IsBestseller,
            product.IsNew);
    }
} 