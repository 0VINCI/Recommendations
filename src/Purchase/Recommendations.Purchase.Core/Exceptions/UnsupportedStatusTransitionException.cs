namespace Recommendations.Purchase.Core.Exceptions;

public sealed class UnsupportedStatusTransitionException(): CustomException("Unsupported status transition.");