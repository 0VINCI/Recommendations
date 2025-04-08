namespace Recommendations.Authorization.Application.DTO;

public sealed record SignUpDto(string Name, string Surname, string Email,  string Password);