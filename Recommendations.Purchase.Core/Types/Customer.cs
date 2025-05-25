namespace Recommendations.Purchase.Core.Types;

public record Customer
{
    public Guid IdCustomer { get; }
    public Guid UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string PhoneNumber { get; }

    private List<Address> Addresses { get; init; }

    private Customer(
        Guid idCustomer,
        Guid userId,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        IEnumerable<Address> addresses)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format.", nameof(email));
        if (!IsValidPhone(phoneNumber))
            throw new ArgumentException("Invalid phone number.", nameof(phoneNumber));
        IdCustomer = idCustomer;
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Addresses = addresses?.ToList() ?? new List<Address>();
    }

    private static bool IsValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && email.Contains('@');

    private static bool IsValidPhone(string phone) =>
        !string.IsNullOrWhiteSpace(phone) && phone.Length >= 7;

    public Customer AddAddress(Address address)
        => this with { Addresses = Addresses.Append(address).ToList() };


    public static Customer Create(Guid userId, string firstName, string lastName, string email, string phoneNumber, 
        IEnumerable<Address>? addresses = null)
    {
        return new Customer(Guid.NewGuid(), userId, firstName, lastName, email, phoneNumber,
            addresses ?? Enumerable.Empty<Address>());
    }
}