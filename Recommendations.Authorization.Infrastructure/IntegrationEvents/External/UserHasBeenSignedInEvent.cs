using Recommendations.Shared.Abstractions.Events;

namespace Recommendations.Authorization.Infrastructure.IntegrationEvents.External;

public sealed record UserHasBeenSignedInEvent : IEvent;