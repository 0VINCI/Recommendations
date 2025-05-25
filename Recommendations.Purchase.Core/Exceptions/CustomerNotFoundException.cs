namespace Recommendations.Purchase.Core.Exceptions;

public sealed class CustomerNotFoundException() : CustomException("No customer found for current user.");