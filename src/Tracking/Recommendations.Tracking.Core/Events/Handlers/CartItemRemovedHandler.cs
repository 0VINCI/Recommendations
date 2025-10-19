using System.Text.Json;
using Recommendations.Shared.Abstractions.Events;
using Recommendations.Tracking.Core.Data.Repositories;
using Recommendations.Tracking.Shared.Events;

namespace Recommendations.Tracking.Core.Events.Handlers;

internal sealed class CartItemRemovedHandler(ITrackingRepository trackingRepository) : IEventHandler<CartItemRemoved>
{
    public async Task HandleAsync(CartItemRemoved @event, CancellationToken cancellationToken = default)
    {
        var context = JsonSerializer.Serialize(new
        {
            source = "Backend",
            module = "Cart",
            action = "RemoveItem"
        });

        var payload = JsonSerializer.Serialize(new
        {
            item_id = @event.ProductId.ToString(),
            product_name = @event.ProductName,
            product_price = @event.ProductPrice,
            quantity = @event.Quantity,
            total = @event.ProductPrice * @event.Quantity
        });

        await trackingRepository.AddEventAsync(new Core.Types.EventRaw
        {
            Id = Guid.NewGuid(),
            Ts = @event.Timestamp,
            Type = "cart_item_removed",
            Source = Core.Types.EventSource.Backend,
            UserId = @event.UserId.ToString(),
            Context = JsonDocument.Parse(context),
            Payload = JsonDocument.Parse(payload),
            ReceivedAt = DateTimeOffset.UtcNow
        }, cancellationToken);
    }
}
