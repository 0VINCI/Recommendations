using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Core.Types;
using Recommendations.Dictionaries.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Dictionaries.Application.Commands.Handlers;

internal sealed class UpdateProductHandler(IProductRepository productRepository) : ICommandHandler<UpdateProduct>
{
    public async Task HandleAsync(UpdateProduct command, CancellationToken cancellationToken = default)
    {
        var existingProduct = await productRepository.GetByIdAsync(command.Product.Id);
        if (existingProduct == null)
            throw new InvalidOperationException($"Product with ID {command.Product.Id} not found");

        var updatedProduct = new Product(
            command.Product.Id,
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

        await productRepository.UpdateAsync(updatedProduct);
    }
} 