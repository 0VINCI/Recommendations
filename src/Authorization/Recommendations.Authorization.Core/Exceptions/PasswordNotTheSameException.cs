using Recommendations.Shared.Infrastructure.Exceptions;

namespace Recommendations.Authorization.Core.Exceptions;

public sealed class PasswordNotTheSameException() : HumanPresentableException("Passwords are not the same.", ExceptionCategory.ValidationError);