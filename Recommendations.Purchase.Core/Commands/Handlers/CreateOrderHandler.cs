using Recommendations.Purchase.Core.Data.Models;
using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Core.Exceptions;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Commands.Handlers;

internal sealed class CreateOrderHandler(IPurchaseRepository purchaseRepository, IUserContext userContext) : ICommandHandler<CreateOrder>
{
    public async Task HandleAsync(CreateOrder command, CancellationToken cancellationToken = default)
    {
        var customer = await purchaseRepository.GetCustomer(userContext.UserId, cancellationToken);
        if (customer is null)
            throw new ClientNotFoundException();

        var orderItems = command.OrderDto.Items
            .Select(i => new OrderItem(
                i.IdOrderItem,
                i.ProductId,
                i.ProductName,
                i.ProductPrice,
                i.Quantity
            )).ToList();

        var order = Order.Create(
            customer.IdCustomer,
            orderItems,
            command.OrderDto.ShippingAddressId
        );

        await purchaseRepository.AddNewOrder(order, cancellationToken);
    }
}