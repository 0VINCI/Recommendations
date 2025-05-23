namespace Recommendations.Purchase.Core.Types;

public record class Order
{
    public Guid IdOrder { get; }
    public Guid CustomerId { get; }
    public IReadOnlyList<OrderItem> Items { get; }
    public Guid ShippingAddressId { get; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? PaidAt { get; private set; }

    private Order(Guid idOrder, Guid customerId, IReadOnlyCollection<OrderItem> items, Guid shippingAddressId)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Order must contain at least one item.", nameof(items));
        IdOrder = idOrder;
        CustomerId = customerId;
        Items = items.ToList();
        ShippingAddressId = shippingAddressId;
        Status = OrderStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid(DateTime paidAt)
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Order must be in 'Created' state to mark as paid.");
        Status = OrderStatus.Paid;
        PaidAt = paidAt;
    }

    public decimal GetTotalAmount() => Items.Sum(x => x.ProductPrice * x.Quantity);
    
    public static Order Create(Guid customerId, IEnumerable<OrderItem> items, Guid shippingAddressId)
    {
        return new Order(Guid.NewGuid(), customerId, items.ToList(), shippingAddressId);
    }
}
