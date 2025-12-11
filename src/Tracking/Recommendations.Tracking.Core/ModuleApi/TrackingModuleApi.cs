using System.Text.Json;
using Recommendations.Tracking.Core.Data.Repositories;
using Recommendations.Tracking.Core.Types;
using Recommendations.Tracking.Shared;

namespace Recommendations.Tracking.Core.ModuleApi;

public class TrackingModuleApi(ITrackingRepository trackingRepository) : ITrackingModuleApi
{
    public async Task<Guid> TrackEventAsync(
        string eventType,
        string source,
        string? userId = null,
        Guid? anonymousId = null,
        Guid? sessionId = null,
        string? context = null,
        string? payload = null,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<EventSource>(source, ignoreCase: true, out var eventSource))
        {
            throw new ArgumentException($"Invalid source: {source}. Must be 'frontend' or 'backend' (case-insensitive)", nameof(source));
        }

        var eventRaw = new EventRaw
        {
            Id = Guid.NewGuid(),
            Ts = DateTimeOffset.UtcNow,
            Type = eventType,
            Source = eventSource,
            UserId = userId,
            AnonymousId = anonymousId,
            SessionId = sessionId,
            Context = JsonDocument.Parse(context ?? "{}"),
            Payload = JsonDocument.Parse(payload ?? "{}"),
            ReceivedAt = DateTimeOffset.UtcNow
        };

        return await trackingRepository.AddEventAsync(eventRaw, cancellationToken);
    }

    public async Task LinkIdentityAsync(Guid anonymousId, string userId, CancellationToken cancellationToken = default)
    {
        // Check if link already exists to avoid duplicate key violations
        var exists = await trackingRepository.IdentityLinkExistsAsync(anonymousId, userId, cancellationToken);
        if (exists)
        {
            return; // Link already exists, nothing to do
        }

        var identityLink = new IdentityLink
        {
            AnonymousId = anonymousId,
            UserId = userId
        };

        await trackingRepository.AddIdentityLinkAsync(identityLink, cancellationToken);
    }

    public async Task<bool> IdentityLinkExistsAsync(Guid anonymousId, string userId, CancellationToken cancellationToken = default)
    {
        return await trackingRepository.IdentityLinkExistsAsync(anonymousId, userId, cancellationToken);
    }
}