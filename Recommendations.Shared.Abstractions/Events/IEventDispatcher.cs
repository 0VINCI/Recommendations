namespace Recommendations.Shared.Abstractions.Events;

    public interface IEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
