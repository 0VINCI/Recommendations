namespace Recommendations.Purchase.Core.Types;

public class OrderItem
{
    public Guid IdOrderItem { get; }
    public Guid ProductId { get; }
    public string ProductName { get; }
    public decimal ProductPrice { get; }
    public int Quantity { get; }

    public OrderItem(Guid idOrderItem, Guid productId, string productName, decimal productPrice, int quantity)
    {
        if (idOrderItem == Guid.Empty)
            throw new ArgumentException("Order item ID cannot be empty.", nameof(idOrderItem));
        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required.", nameof(productName));
        if (productPrice < 0)
            throw new ArgumentException("Product price cannot be negative.", nameof(productPrice));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        IdOrderItem = idOrderItem;
        ProductId = productId;
        ProductName = productName;
        ProductPrice = productPrice;
        Quantity = quantity;
    }
    public static OrderItem Create(Guid productId, string productName, decimal productPrice, int quantity)
    {
        return new OrderItem(Guid.NewGuid(), productId, productName, productPrice, quantity);
    }

    public decimal GetTotalPrice() => ProductPrice * Quantity;
}