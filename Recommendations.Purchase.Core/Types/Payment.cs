using Recommendations.Purchase.Core.Types.Enums;

namespace Recommendations.Purchase.Core.Types;

public record Payment
{
    public Guid IdPayment { get; }
    public PaymentMethod Method { get; }
    public DateTime PaymentDate { get; }
    public string Details { get; }

    private Payment(Guid idPayment, PaymentMethod method, DateTime paymentDate, string details)
    {
        if (method == PaymentMethod.Undefined)
            throw new ArgumentException("Payment method must be specified.", nameof(method));
        if (paymentDate > DateTime.UtcNow)
            throw new ArgumentException("Payment date cannot be in the future.", nameof(paymentDate));
        if (string.IsNullOrWhiteSpace(details))
            throw new ArgumentException("Payment details required.", nameof(details));
        IdPayment = idPayment;
        Method = method;
        PaymentDate = paymentDate;
        Details = details;
    }
    public static Payment Create(PaymentMethod method, DateTime paymentDate, string details)
    {
        return new Payment(Guid.NewGuid(), method, paymentDate, details);
    }
}