using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Core.Types;
using Recommendations.Dictionaries.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Dictionaries.Application.Commands.Handlers;

internal sealed class AddProductHandler(IProductRepository productRepository) : ICommandHandler<AddProduct>
{
    public async Task HandleAsync(AddProduct command, CancellationToken cancellationToken = default)
    {
        var product = Product.Create(
            command.ProductDto.ProductDisplayName,
            command.ProductDto.BrandName,
            command.ProductDto.Price,
            command.ProductDto.OriginalPrice,
            command.ProductDto.Rating,
            command.ProductDto.Reviews,
            command.ProductDto.IsBestseller,
            command.ProductDto.IsNew,
            command.ProductDto.SubCategoryId,
            command.ProductDto.ArticleTypeId,
            command.ProductDto.BaseColourId);

        await productRepository.AddAsync(product);
    }
} 