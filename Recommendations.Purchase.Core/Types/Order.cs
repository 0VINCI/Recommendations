using Recommendations.Purchase.Core.Types.Enums;

namespace Recommendations.Purchase.Core.Types;

public class Order
{
    public Guid IdOrder { get; }
    public Guid CustomerId { get; }
    private IReadOnlyList<OrderItem> Items { get; }
    public Guid ShippingAddressId { get; }
    private OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; }
    public DateTime? PaidAt { get; private set; }
    private List<Payment> Payments { get; init; }

    private Order(Guid idOrder, Guid customerId, IReadOnlyCollection<OrderItem> items, 
        Guid shippingAddressId, IEnumerable<Payment> payments)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Order must contain at least one item.", nameof(items));
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
            throw new InvalidOperationException("Order must be in 'Created' state to mark as paid.");
        Status = OrderStatus.Paid;
        PaidAt = paidAt;
    }
    public void AddPayment(Payment payment)
    {
        Payments.Add(payment);
    }

    public decimal GetTotalAmount() => Items.Sum(x => x.ProductPrice * x.Quantity);
    
    public static Order Create(Guid customerId, IEnumerable<OrderItem> items, Guid shippingAddressId, IEnumerable<Payment>? payments = null)
    {
        return new Order(Guid.NewGuid(), customerId, items.ToList(), shippingAddressId, payments ?? Enumerable.Empty<Payment>());
    }
}
