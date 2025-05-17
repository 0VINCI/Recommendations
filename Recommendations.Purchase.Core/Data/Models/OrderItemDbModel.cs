namespace Recommendations.Purchase.Core.Data.Models;

public class OrderItemDbModel
{
    public Guid IdOrderItem { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
}