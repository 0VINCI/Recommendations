using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Core.Exceptions;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Commands.Handlers;

internal sealed class UpdateCustomerHandler(IPurchaseRepository purchaseRepository, IUserContext userContext) : ICommandHandler<UpdateCustomer>
{
    public async Task HandleAsync(UpdateCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await purchaseRepository.GetCustomer(userContext.UserId, cancellationToken);
        if (customer is null)
            throw new CustomerNotFoundException();

        var data = command.CustomerDto;

        customer.UpdateDetails(data.FirstName, data.LastName, data.Email, data.PhoneNumber);
        
        await purchaseRepository.SaveCustomer(customer, cancellationToken);
    }
}