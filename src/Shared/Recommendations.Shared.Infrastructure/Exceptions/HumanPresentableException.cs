namespace Recommendations.Shared.Infrastructure.Exceptions;

public abstract class HumanPresentableException(string message, ExceptionCategory exceptionCategory) : Exception(message)
{
    public ExceptionCategory ExceptionCategory { get; } = exceptionCategory;
}

public enum ExceptionCategory
{
    ValidationError,
    NotFound,
    AlreadyExists,
    ConcurrencyError,
    TechnicalError,
    Unauthorized
} 