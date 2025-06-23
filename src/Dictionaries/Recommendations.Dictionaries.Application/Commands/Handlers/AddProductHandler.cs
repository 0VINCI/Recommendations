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
            command.Product.ProductDisplayName,
            command.Product.BrandName,
            command.Product.Price,
            command.Product.OriginalPrice,
            command.Product.Rating,
            command.Product.Reviews,
            command.Product.IsBestseller,
            command.Product.IsNew,
            command.Product.SubCategoryId,
            command.Product.ArticleTypeId,
            command.Product.BaseColourId);

        await productRepository.AddAsync(product);
    }
} 