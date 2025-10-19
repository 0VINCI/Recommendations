namespace Recommendations.Tracking.Shared;

public interface ITrackingModuleApi
{
    Task<Guid> TrackEventAsync(
        string eventType,
        string source,
        string? userId = null,
        Guid? anonymousId = null,
        Guid? sessionId = null,
        string? context = null,
        string? payload = null,
        CancellationToken cancellationToken = default);

    Task LinkIdentityAsync(Guid anonymousId, string userId, CancellationToken cancellationToken = default);
    Task<bool> IdentityLinkExistsAsync(Guid anonymousId, string userId, CancellationToken cancellationToken = default);
}