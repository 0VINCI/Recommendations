using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Core.Types.Enums;

namespace Recommendations.Purchase.Core.Data.Models;

public class OrderDbModel
{
    public Guid IdOrder { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ShippingAddressId { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }

    public ICollection<OrderItemDbModel> Items { get; set; } = new List<OrderItemDbModel>();
    public ICollection<PaymentDbModel> Payments { get; set; } = new List<PaymentDbModel>();

}