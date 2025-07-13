namespace Recommendations.Authorization.Shared.DTO;

public sealed record SignedInDto(Guid IdUser, string Name, string Surname, string Email, string Token);