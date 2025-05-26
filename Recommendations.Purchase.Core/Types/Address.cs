namespace Recommendations.Purchase.Core.Types;

public class Address
{
    public Guid IdAddress { get; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }

    private Address(Guid idAddress, string street, string city, string postalCode, string country)
    {
        IdAddress = idAddress;
        UpdateAddress(street, city, postalCode, country);
    }
    public void UpdateAddress(string street, string city, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street is required.", nameof(street));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code is required.", nameof(postalCode));
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required.", nameof(country));

        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }
    public static Address Create(string street, string city, string postalCode, string country)
    {
        return new Address(Guid.NewGuid(), street, city, postalCode, country);
    }
}