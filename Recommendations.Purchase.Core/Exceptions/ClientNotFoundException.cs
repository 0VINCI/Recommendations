namespace Recommendations.Purchase.Core.Exceptions;

public sealed class ClientNotFoundException() : CustomException("No customer found for current user.");