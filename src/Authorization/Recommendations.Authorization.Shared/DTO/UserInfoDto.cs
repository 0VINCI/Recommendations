namespace Recommendations.Authorization.Shared.DTO;

public sealed record UserInfoDto(Guid IdUser, string Name, string Surname, string Email);