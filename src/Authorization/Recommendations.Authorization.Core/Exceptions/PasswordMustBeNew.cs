using Recommendations.Shared.Infrastructure.Exceptions;

namespace Recommendations.Authorization.Core.Exceptions;

public sealed class PasswordMustBeNew() : HumanPresentableException("New password must be different from the current password.", ExceptionCategory.ValidationError);