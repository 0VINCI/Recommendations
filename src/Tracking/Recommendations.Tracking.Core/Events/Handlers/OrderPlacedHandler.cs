using System.Text.Json;
using Recommendations.Shared.Abstractions.Events;
using Recommendations.Tracking.Core.Data.Repositories;
using Recommendations.Tracking.Core.Types;
using Recommendations.Tracking.Shared.Events;

namespace Recommendations.Tracking.Core.Events.Handlers;

internal sealed class OrderPlacedHandler(ITrackingRepository trackingRepository) : IEventHandler<OrderPlaced>
{
    public async Task HandleAsync(OrderPlaced @event, CancellationToken cancellationToken = default)
    {
        var context = JsonSerializer.Serialize(new
        {
            source = "Backend",
            module = "Purchase",
            action = "CreateOrder"
        });

        var payload = JsonSerializer.Serialize(new
        {
            order_id = @event.OrderId.ToString(),
            total_amount = @event.TotalAmount,
            items_count = @event.ItemsCount,
            items = @event.Items.Select(i => new { 
                item_id = i.ProductId.ToString(), 
                quantity = i.Quantity,
                price = i.ProductPrice 
            }).ToArray()
        });
        
        await trackingRepository.AddEventAsync(new EventRaw()
        {
            Id = Guid.NewGuid(),
            Ts = @event.Timestamp,
            Type = "order_placed",
            Source = EventSource.Backend,
            UserId = @event.UserId.ToString(),
            Context = JsonDocument.Parse(context),
            Payload = JsonDocument.Parse(payload),
            ReceivedAt = DateTimeOffset.UtcNow
        }, cancellationToken);
    }
}
