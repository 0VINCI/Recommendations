namespace Recommendations.Purchase.Core.Data.Models;

public class AddressDbModel
{
    public Guid IdAddress { get; set; }
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string Country { get; set; } = default!;
}