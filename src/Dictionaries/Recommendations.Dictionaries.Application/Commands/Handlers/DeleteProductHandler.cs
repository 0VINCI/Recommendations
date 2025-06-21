using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Dictionaries.Application.Commands.Handlers;

internal sealed class DeleteProductHandler(IProductRepository productRepository) : ICommandHandler<DeleteProduct>
{
    public async Task HandleAsync(DeleteProduct command, CancellationToken cancellationToken = default)
    {
        var exists = await productRepository.ExistsAsync(command.Id);
        if (!exists)
            throw new InvalidOperationException($"Product with ID {command.Id} not found");

        await productRepository.DeleteAsync(command.Id);
    }
} 