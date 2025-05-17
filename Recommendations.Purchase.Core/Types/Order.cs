namespace Recommendations.Purchase.Core.Types;

public record class Order
{
    public Guid Id { get; }
    public Guid CustomerId { get; }
    public IReadOnlyList<OrderItem> Items { get; }
    public Guid ShippingAddressId { get; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? PaidAt { get; private set; }

    public Order(Guid id, Guid customerId, IEnumerable<OrderItem> items, Guid shippingAddressId)
    {
        if (items == null || !items.Any())
            throw new ArgumentException("Order must contain at least one item.", nameof(items));
        Id = id;
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
}
