namespace Recommendations.Purchase.Shared.DTO;

public record PaymentDto(
    Guid IdPayment, 
    uint Method, 
    DateTime PaymentDate, 
    string Details    
    );