namespace Recommendations.Authorization.Application.DTO;

public sealed record ChangePasswordDto(string Email, string OldPassword, string NewPassword);