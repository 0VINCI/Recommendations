using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Core.Exceptions;
using Recommendations.Purchase.Core.Types.Enums;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Core.Commands.Handlers;

internal sealed class UpdateStatusHandler(IPurchaseRepository purchaseRepository) : ICommandHandler<UpdateStatus>
{
    public async Task HandleAsync(UpdateStatus command, CancellationToken cancellationToken = default)
    {
        var order = await purchaseRepository.GetOrderById(command.OrderId, cancellationToken);
        if (order is null)
            throw new OrderNotFoundException();

        var newStatus = (OrderStatus)command.Status;

        switch (newStatus)
        {
            case OrderStatus.Paid:
                order.MarkAsPaid(DateTime.UtcNow);
                break;
            case OrderStatus.Shipped:
                order.MarkAsShipped(DateTime.UtcNow);
                break;
            case OrderStatus.Delivered:
                order.MarkAsDelivered(DateTime.UtcNow);
                break;
            case OrderStatus.Cancelled:
                order.MarkAsCancelled(DateTime.UtcNow);
                break;
            case OrderStatus.Created:
            default:
                throw new UnsupportedStatusTransitionException();
        }
        
        await purchaseRepository.SaveOrder(order, cancellationToken);
    }
}