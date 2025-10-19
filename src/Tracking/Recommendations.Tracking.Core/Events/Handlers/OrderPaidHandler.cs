using System.Text.Json;
using Recommendations.Shared.Abstractions.Events;
using Recommendations.Tracking.Core.Data.Repositories;
using Recommendations.Tracking.Core.Types;
using Recommendations.Tracking.Shared.Events;

namespace Recommendations.Tracking.Core.Events.Handlers;

internal sealed class OrderPaidHandler(ITrackingRepository trackingRepository) : IEventHandler<OrderPaid>
{
    public async Task HandleAsync(OrderPaid @event, CancellationToken cancellationToken = default)
    {
        var context = JsonSerializer.Serialize(new
        {
            source = "Backend",
            module = "Purchase",
            action = "PayForOrder"  
        });

        var payload = JsonSerializer.Serialize(new
        {
            order_id = @event.OrderId.ToString(),
            total_amount = @event.TotalAmount,
            payment_method = @event.PaymentMethod
        });

        await trackingRepository.AddEventAsync(new EventRaw()
        {
            Id = Guid.NewGuid(),
            Ts = @event.Timestamp,
            Type = "order_paid",
            Source = EventSource.Backend,
            UserId = @event.UserId.ToString(),
            Context = JsonDocument.Parse(context),
            Payload = JsonDocument.Parse(payload),
            ReceivedAt = DateTimeOffset.UtcNow
        }, cancellationToken);
    }
}
