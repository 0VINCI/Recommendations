namespace Recommendations.Tracking.Shared.DTO;

public record LinkIdentityRequest(
    Guid AnonymousId,
    string UserId
);
