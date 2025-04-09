namespace Recommendations.Authorization.Application.Exceptions;

public sealed class UserAlreadyExistsException() : CustomException("User already exists.");