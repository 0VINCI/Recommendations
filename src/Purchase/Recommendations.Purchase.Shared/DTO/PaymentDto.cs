namespace Recommendations.Purchase.Shared.DTO;

public record PaymentDto(Guid OrderId, uint Method, DateTime PaymentDate, string Details);