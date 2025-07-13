using Recommendations.Shared.Infrastructure.Exceptions;

namespace Recommendations.Authorization.Core.Exceptions;

public sealed class UserNotFoundException() : HumanPresentableException("User not found.", ExceptionCategory.NotFound);
