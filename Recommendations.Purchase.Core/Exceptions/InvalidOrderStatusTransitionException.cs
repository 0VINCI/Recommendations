using Recommendations.Purchase.Core.Types.Enums;

namespace Recommendations.Purchase.Core.Exceptions;

public sealed class InvalidOrderStatusTransitionException(OrderStatus current, OrderStatus desired) 
    : CustomException($"Cannot change order status from '{current}' to '{desired}'.");