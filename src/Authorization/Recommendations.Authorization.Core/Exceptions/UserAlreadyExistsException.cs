using Recommendations.Shared.Infrastructure.Exceptions;

namespace Recommendations.Authorization.Core.Exceptions;

public sealed class UserAlreadyExistsException() : HumanPresentableException("User already exists.", ExceptionCategory.AlreadyExists);