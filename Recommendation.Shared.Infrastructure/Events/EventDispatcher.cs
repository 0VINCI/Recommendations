using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Abstractions.Events;

namespace Recommendation.Shared.Infrastructure.Events;

internal sealed class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
    {
        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            using var scope = serviceProvider.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();

            var tasks = handlers.Select(x => x.HandleAsync(@event));
            await Task.WhenAll(tasks);
        }
    }
