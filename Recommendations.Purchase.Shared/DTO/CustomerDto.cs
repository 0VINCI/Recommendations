namespace Recommendations.Purchase.Shared.DTO;

public record CustomerDto(
    Guid IdCustomer,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    Guid UserId,
    IReadOnlyCollection<AddressDto> Addresses,
    IReadOnlyCollection<PaymentDto> Payments
    );