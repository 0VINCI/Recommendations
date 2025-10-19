using System.Text.Json;
using Recommendations.Shared.Abstractions.Events;
using Recommendations.Tracking.Core.Data.Repositories;
using Recommendations.Tracking.Shared.Events;

namespace Recommendations.Tracking.Core.Events.Handlers;

internal sealed class CartItemQuantityChangedHandler(ITrackingRepository trackingRepository) : IEventHandler<CartItemQuantityChanged>
{
    public async Task HandleAsync(CartItemQuantityChanged @event, CancellationToken cancellationToken = default)
    {
        var context = JsonSerializer.Serialize(new
        {
            source = "Backend",
            module = "Cart",
            action = "UpdateQuantity"
        });

        var payload = JsonSerializer.Serialize(new
        {
            item_id = @event.ProductId.ToString(),
            product_name = @event.ProductName,
            product_price = @event.ProductPrice,
            old_quantity = @event.OldQuantity,
            new_quantity = @event.NewQuantity,
            quantity_change = @event.NewQuantity - @event.OldQuantity
        });

        await trackingRepository.AddEventAsync(new Core.Types.EventRaw
        {
            Id = Guid.NewGuid(),
            Ts = @event.Timestamp,
            Type = "cart_item_quantity_changed",
            Source = Core.Types.EventSource.Backend,
            UserId = @event.UserId.ToString(),
            Context = JsonDocument.Parse(context),
            Payload = JsonDocument.Parse(payload),
            ReceivedAt = DateTimeOffset.UtcNow
        }, cancellationToken);
    }
}
