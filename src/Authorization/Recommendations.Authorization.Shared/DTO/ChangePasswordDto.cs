namespace Recommendations.Authorization.Shared.DTO;

public record ChangePasswordDto(string Email, string OldPassword, string NewPassword);