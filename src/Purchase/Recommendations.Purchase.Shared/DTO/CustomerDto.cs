namespace Recommendations.Purchase.Shared.DTO;

public record CustomerDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string UserId,
    Guid? IdCustomer = null,
    List<AddressDto>? Addresses = null);