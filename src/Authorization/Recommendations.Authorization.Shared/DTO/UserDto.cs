namespace Recommendations.Authorization.Shared.DTO;

public sealed record UserDto(Guid IdUser, string Name, string Surname, string Email);