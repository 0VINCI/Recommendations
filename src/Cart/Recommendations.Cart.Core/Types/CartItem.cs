namespace Recommendations.Cart.Core.Types;
public sealed class CartItem
{
    public Guid ProductId  { get; private set; }
    public string Name     { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity    { get; private set; }

    public decimal Subtotal => UnitPrice * Quantity;

    public CartItem(Guid productId, string name, decimal unitPrice, int quantity = 1)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be > 0", nameof(quantity));
        if (unitPrice < 0)  throw new ArgumentException("Price must be >= 0", nameof(unitPrice));

        ProductId  = productId;
        Name       = name;
        UnitPrice  = unitPrice;
        Quantity   = quantity;
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be > 0", nameof(amount));
        Quantity += amount;
    }

    public void ChangeQuantity(int newQuantity)
    {
        if (newQuantity <= 0) throw new ArgumentException("Quantity must be > 0", nameof(newQuantity));
        Quantity = newQuantity;
    }
}