namespace Recommendations.Purchase.Core.Exceptions;

public sealed class EmptyOrderItemsException() : CustomException("Order must contain at least one item.");