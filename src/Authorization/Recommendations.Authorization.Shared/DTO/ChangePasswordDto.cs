namespace Recommendations.Authorization.Shared.DTO;

public sealed record ChangePasswordDto(string Email, string OldPassword, string NewPassword);