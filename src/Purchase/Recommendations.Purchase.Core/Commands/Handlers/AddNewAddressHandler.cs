using Recommendations.Purchase.Core.Data.Models;
using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Core.Exceptions;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Commands.Handlers;

internal sealed class AddNewAddressHandler(IPurchaseRepository purchaseRepository, 
    IUserContext userContext) : ICommandHandler<AddNewAddress>
{
    public async Task HandleAsync(AddNewAddress command, CancellationToken cancellationToken = default)
    {
        var customer = await purchaseRepository.GetCustomer(userContext.UserId, cancellationToken);
        if (customer is null)
            throw new CustomerNotFoundException();

        var data = command.AddressDto;
        var address = Address.Create(data.Street, data.City, data.PostalCode, data.Country);
        
        customer.AddAddress(address);
        await purchaseRepository.SaveCustomer(customer, cancellationToken);
    }
}