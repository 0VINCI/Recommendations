using System.Text.Json;
using Recommendations.Shared.Abstractions.Events;
using Recommendations.Tracking.Core.Data.Repositories;
using Recommendations.Tracking.Shared.Events;

namespace Recommendations.Tracking.Core.Events.Handlers;

internal sealed class CartClearedHandler(ITrackingRepository trackingRepository) : IEventHandler<CartCleared>
{
    public async Task HandleAsync(CartCleared @event, CancellationToken cancellationToken = default)
    {
        var context = JsonSerializer.Serialize(new
        {
            source = "Backend",
            module = "Cart",
            action = "ClearCart"
        });

        var payload = JsonSerializer.Serialize(new
        {
            // No specific item data for cart cleared
        });

        await trackingRepository.AddEventAsync(new Core.Types.EventRaw
        {
            Id = Guid.NewGuid(),
            Ts = @event.Timestamp,
            Type = "cart_cleared",
            Source = Core.Types.EventSource.Backend,
            UserId = @event.UserId.ToString(),
            Context = JsonDocument.Parse(context),
            Payload = JsonDocument.Parse(payload),
            ReceivedAt = DateTimeOffset.UtcNow
        }, cancellationToken);
    }
}
