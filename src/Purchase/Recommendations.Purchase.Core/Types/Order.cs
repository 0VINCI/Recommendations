using Recommendations.Purchase.Core.Exceptions;
using Recommendations.Purchase.Core.Types.Enums;

namespace Recommendations.Purchase.Core.Types;

public class Order
{
    public Guid IdOrder { get; }
    public Guid CustomerId { get; }
    internal IReadOnlyList<OrderItem> Items { get; }
    public Guid ShippingAddressId { get; }
    internal OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; }
    public DateTime? PaidAt { get; private set; }
    internal List<Payment> Payments { get; init; }

    private Order(Guid idOrder, Guid customerId, IReadOnlyCollection<OrderItem> items, 
        Guid shippingAddressId, IEnumerable<Payment> payments)
    {
        if (items == null || items.Count == 0)
            throw new EmptyOrderItemsException();
        
        IdOrder = idOrder;
        CustomerId = customerId;
        Items = items.ToList();
        ShippingAddressId = shippingAddressId;
        Status = OrderStatus.Created;
        CreatedAt = DateTime.UtcNow;
        Payments = payments?.ToList() ?? new List<Payment>();
    }

    public void MarkAsPaid(DateTime paidAt)
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOrderStatusTransitionException(Status, OrderStatus.Paid);
        Status = OrderStatus.Paid;
        PaidAt = paidAt;
    }
    public void MarkAsShipped(DateTime dateTime)
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOrderStatusTransitionException(Status, OrderStatus.Shipped);
        Status = OrderStatus.Shipped;
    }
    public void MarkAsDelivered(DateTime dateTime)
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOrderStatusTransitionException(Status, OrderStatus.Delivered);
        Status = OrderStatus.Delivered;
    }
    
    public void MarkAsCancelled(DateTime dateTime)
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOrderStatusTransitionException(Status, OrderStatus.Cancelled);
        Status = OrderStatus.Cancelled;
    }

    public void AddPayment(Payment payment)
    {
        Payments.Add(payment);
    }

    public decimal GetTotalAmount() => Items.Sum(x => x.ProductPrice * x.Quantity);
    public int GetItemsCount() => Items.Count;
    
    public static Order Create(Guid customerId, IEnumerable<OrderItem> items, Guid shippingAddressId, IEnumerable<Payment>? payments = null)
    {
        return new Order(Guid.NewGuid(), customerId, items.ToList(), shippingAddressId, payments ?? Enumerable.Empty<Payment>());
    }
}