namespace Recommendations.Shared.Infrastructure.Exceptions;

public sealed class FailedSendEmailException(string content) : CustomException($"Failed to send email: {content}");