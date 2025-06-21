using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Core.Exceptions;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Core.Types.Enums;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Core.Commands.Handlers;

internal sealed class PayForOrderHandler(IPurchaseRepository purchaseRepository) : ICommandHandler<PayForOrder>
{
    public async Task HandleAsync(PayForOrder command, CancellationToken cancellationToken = default)
    {
        var data = command.PaymentDto;
        var order = await purchaseRepository.GetOrderById(data.OrderId, cancellationToken);
        
        if (order is null)
            throw new OrderNotFoundException();
        
        var payment = Payment.Create((PaymentMethod)data.Method, data.PaymentDate, data.Details);
        
        order.AddPayment(payment);
        order.MarkAsPaid(data.PaymentDate);

        await purchaseRepository.SaveOrder(order, cancellationToken);
    }
}