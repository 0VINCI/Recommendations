namespace Recommendations.Authorization.Application.Exceptions;

public sealed class PasswordNotTheSameException() : CustomException("Passwords are not the same.");