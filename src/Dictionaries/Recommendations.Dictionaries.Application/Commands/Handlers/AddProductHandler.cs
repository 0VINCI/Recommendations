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
            command.Product.Name,
            command.Product.Price,
            command.Product.OriginalPrice,
            command.Product.Image,
            command.Product.Category,
            command.Product.Description,
            command.Product.Sizes,
            command.Product.Colors,
            command.Product.Rating,
            command.Product.Reviews,
            command.Product.IsBestseller,
            command.Product.IsNew);

        await productRepository.AddAsync(product);
    }
} 