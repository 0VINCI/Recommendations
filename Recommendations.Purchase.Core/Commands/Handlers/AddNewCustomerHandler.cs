using Recommendations.Purchase.Core.Data.Models;
using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Core.Exceptions;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Core.Types.Enums;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Commands.Handlers;

internal sealed class AddNewCustomerHandler(IPurchaseRepository purchaseRepository, IUserContext userContext) : ICommandHandler<AddNewCustomer>
{
    public async Task HandleAsync(AddNewCustomer command, CancellationToken cancellationToken = default)
    {
        var isExists = await purchaseRepository.GetCustomer(userContext.UserId, cancellationToken);
        if (isExists is not null)
            throw new CustomerAlreadyExistsException();

        var data = command.CustomerDto;
        
        var addresses = data.Addresses?
            .Select(a => Address.Create(a.Street, a.City, a.PostalCode, a.Country))
            .ToList() ?? new List<Address>();
        
        var newCustomer = Customer.Create(userContext.UserId,data.FirstName, data.LastName, 
            data.Email, data.PhoneNumber, addresses);
   
        await purchaseRepository.AddNewCustomer(newCustomer, cancellationToken);
    }
}