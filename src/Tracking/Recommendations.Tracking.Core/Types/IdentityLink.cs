namespace Recommendations.Tracking.Core.Types;

public sealed class IdentityLink
{
    public Guid AnonymousId { get; set; }
    public string UserId { get; set; } = default!;
    public DateTimeOffset LinkedAt { get; set; } = DateTimeOffset.UtcNow;
}
