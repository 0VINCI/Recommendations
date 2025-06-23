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

        existingProduct.UpdateRating(command.Product.Rating, command.Product.Reviews);
        existingProduct.MarkAsBestseller(command.Product.IsBestseller);
        existingProduct.MarkAsNew(command.Product.IsNew);
        existingProduct.UpdatePrice(command.Product.Price, command.Product.OriginalPrice);

        await productRepository.UpdateAsync(existingProduct);
    }
} 