namespace Recommendations.Authorization.Shared.DTO;

public sealed record SignedInDto(Guid IdUser,  string Token);