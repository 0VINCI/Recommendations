using Recommendations.Purchase.Core.Types;

namespace Recommendations.Purchase.Core.Data.Models;

public class PaymentDbModel
{
    public Guid IdPayment { get; set; }
    public PaymentMethod Method { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Details { get; set; } = default!;
}