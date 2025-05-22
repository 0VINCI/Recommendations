namespace Recommendations.Purchase.Core.Types;

public record class Customer
{
    public Guid IdCustomer { get; }
    public Guid UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    
    public IReadOnlyList<Address> Addresses { get; init; }
    public IReadOnlyList<Payment> Payments { get; init; }

    public Customer(
        Guid idCustomer,
        Guid userId,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        IEnumerable<Address> addresses,
        IEnumerable<Payment> payments)
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
        Addresses = addresses?.ToList() ?? throw new ArgumentNullException(nameof(addresses));
        Payments = payments?.ToList() ?? new List<Payment>();
    }

    private static bool IsValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && email.Contains('@');

    private static bool IsValidPhone(string phone) =>
        !string.IsNullOrWhiteSpace(phone) && phone.Length >= 7;

    public Customer AddAddress(Address address)
        => this with { Addresses = Addresses.Append(address).ToList() };

    public Customer AddPayment(Payment payment)
        => this with { Payments = Payments.Append(payment).ToList() };
}