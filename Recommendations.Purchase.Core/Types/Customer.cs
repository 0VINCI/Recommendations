namespace Recommendations.Purchase.Core.Types;

public class Customer
{
    public Guid IdCustomer { get; }
    public Guid UserId { get; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }

    public List<Address> Addresses { get; private set; }

    private Customer(
        Guid idCustomer,
        Guid userId,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        IEnumerable<Address> addresses)
    {
        UpdateDetails(firstName, lastName, email, phoneNumber);
        IdCustomer = idCustomer;
        UserId = userId;
        Addresses = addresses?.ToList() ?? new List<Address>();
    }
    public void UpdateDetails(string firstName, string lastName, string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format.", nameof(email));
        if (!IsValidPhone(phoneNumber))
            throw new ArgumentException("Invalid phone number.", nameof(phoneNumber));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    private static bool IsValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && email.Contains('@');

    private static bool IsValidPhone(string phone) =>
        !string.IsNullOrWhiteSpace(phone) && phone.Length >= 7;

    public void AddAddress(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);
        Addresses.Add(address);
    }

    public static Customer Create(Guid userId, string firstName, string lastName, string email, string phoneNumber, 
        IEnumerable<Address>? addresses = null)
    {
        return new Customer(Guid.NewGuid(), userId, firstName, lastName, email, phoneNumber,
            addresses ?? Enumerable.Empty<Address>());
    }
}