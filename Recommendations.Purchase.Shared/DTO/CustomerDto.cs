namespace Recommendations.Purchase.Shared.DTO;

public record CustomerDto(
    Guid IdCustomer,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    Guid UserId,
    List<AddressDto>? Addresses = null,
    List<PaymentDto>? Payments = null
    );