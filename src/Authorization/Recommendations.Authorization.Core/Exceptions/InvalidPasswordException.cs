using Recommendations.Shared.Infrastructure.Exceptions;

namespace Recommendations.Authorization.Core.Exceptions;

public sealed class InvalidPasswordException() : HumanPresentableException("Invalid email or password.", ExceptionCategory.Unauthorized);