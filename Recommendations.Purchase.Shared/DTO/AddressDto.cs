namespace Recommendations.Purchase.Shared.DTO;

public record AddressDto(
    Guid IdAddress, 
    string Street, 
    string City, 
    string PostalCode, 
    string Country
    );