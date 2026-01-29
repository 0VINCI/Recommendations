using Microsoft.EntityFrameworkCore;
using Recommendations.Tracking.Core.Data.Tracking;
using Recommendations.Tracking.Core.Types;

namespace Recommendations.Tracking.Core.Data.Repositories;

internal sealed class TrackingRepository(TrackingDbContext context) : ITrackingRepository
{
    public async Task<Guid> AddEventAsync(EventRaw eventRaw, CancellationToken cancellationToken = default)
    {
        context.EventsRaw.Add(eventRaw);
        await context.SaveChangesAsync(cancellationToken);
        return eventRaw.Id;
    }

    public async Task AddIdentityLinkAsync(IdentityLink identityLink, CancellationToken cancellationToken = default)
    {
        context.IdentityLinks.Add(identityLink);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRejectedEventAsync(EventRejected eventRejected, CancellationToken cancellationToken = default)
    {
        context.EventsRejected.Add(eventRejected);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<EventRaw?> GetEventByIdAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await context.EventsRaw
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<EventRaw>> GetEventsByUserIdAsync(string userId, DateTimeOffset? from = null, DateTimeOffset? to = null, CancellationToken cancellationToken = default)
    {
        var query = context.EventsRaw
            .AsNoTracking()
            .Where(e => e.UserId == userId);

        if (from.HasValue)
            query = query.Where(e => e.Ts >= from.Value);
        
        if (to.HasValue)
            query = query.Where(e => e.Ts <= to.Value);

        return await query
            .OrderByDescending(e => e.Ts)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<EventRaw>> GetEventsByAnonymousIdAsync(Guid anonymousId, DateTimeOffset? from = null, DateTimeOffset? to = null, CancellationToken cancellationToken = default)
    {
        var query = context.EventsRaw
            .AsNoTracking()
            .Where(e => e.AnonymousId == anonymousId);

        if (from.HasValue)
            query = query.Where(e => e.Ts >= from.Value);
        
        if (to.HasValue)
            query = query.Where(e => e.Ts <= to.Value);

        return await query
            .OrderByDescending(e => e.Ts)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<EventRaw>> GetEventsBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await context.EventsRaw
            .AsNoTracking()
            .Where(e => e.SessionId == sessionId)
            .OrderByDescending(e => e.Ts)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IdentityLinkExistsAsync(Guid anonymousId, string userId, CancellationToken cancellationToken = default)
    {
        return await context.IdentityLinks
            .AsNoTracking()
            .AnyAsync(il => il.AnonymousId == anonymousId && il.UserId == userId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<string>> GetRecentlyViewedProductIdsAsync(string userId, int limit = 10, CancellationToken cancellationToken = default)
    {
        var productIds = await context.EventsRaw
            .AsNoTracking()
            .Where(e => e.UserId == userId && e.Type == "product_viewed" && e.ItemId != null)
            .OrderByDescending(e => e.Ts)
            .Select(e => e.ItemId!)
            .ToListAsync(cancellationToken);

        return productIds
            .Distinct()
            .Take(limit)
            .ToList();
    }
}
