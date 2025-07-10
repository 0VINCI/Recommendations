using Recommendations.Authorization.Application.Exceptions;

namespace Recommendations.Authorization.Core.Exceptions;

public sealed class UserNotFoundException() : CustomException("User not found.");