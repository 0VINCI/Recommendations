namespace Recommendations.Purchase.Core.Data.Models;

public class CustomerDbModel
{
    public Guid IdCustomer { get; set; }
    public Guid UserId { get; set; }   
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;


    public ICollection<AddressDbModel> Addresses { get; set; } = new List<AddressDbModel>();
}