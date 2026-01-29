using Recommendations.Tracking.Core.Types;

namespace Recommendations.Tracking.Core.Data.Repositories;

public interface ITrackingRepository
{
    Task<Guid> AddEventAsync(EventRaw eventRaw, CancellationToken cancellationToken = default);
    Task AddIdentityLinkAsync(IdentityLink identityLink, CancellationToken cancellationToken = default);
    Task AddRejectedEventAsync(EventRejected eventRejected, CancellationToken cancellationToken = default);
    
    Task<EventRaw?> GetEventByIdAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<EventRaw>> GetEventsByUserIdAsync(string userId, DateTimeOffset? from = null, DateTimeOffset? to = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<EventRaw>> GetEventsByAnonymousIdAsync(Guid anonymousId, DateTimeOffset? from = null, DateTimeOffset? to = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<EventRaw>> GetEventsBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<bool> IdentityLinkExistsAsync(Guid anonymousId, string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<string>> GetRecentlyViewedProductIdsAsync(string userId, int limit = 10, CancellationToken cancellationToken = default);
}
