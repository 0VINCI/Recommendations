using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Core.Exceptions;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Core.Types.Enums;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands.CommandWithResult;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Commands.Handlers;

internal sealed class CreateOrderHandler(IPurchaseRepository purchaseRepository, IUserContext userContext) : ICommandHandlerWithResult<CreateOrder, Guid>

{
    public async Task<Guid> HandleAsync(CreateOrder command, CancellationToken cancellationToken = default)
    {
        var customer = await purchaseRepository.GetCustomer(userContext.UserId, cancellationToken);
        
        if (customer is null)
            throw new CustomerNotFoundException();

        var orderItems = command.Items
            .Select(i => OrderItem.Create(
                i.ProductId,
                i.ProductName,
                i.ProductPrice,
                i.Quantity
            )).ToList();
        
        var payments = (command.Payments ?? [])
            .Select(i => new Payment(
                i.OrderId,
                (PaymentMethod)i.Method,
                i.PaymentDate,
                i.Details
            ))
            .ToList();

        var order = Order.Create(
            customer.IdCustomer,
            orderItems,
            command.ShippingAddressId,
            payments
        );

        await purchaseRepository.AddNewOrder(order, cancellationToken);
        return order.IdOrder; 
    }
}