using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Core.Exceptions;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Purchase.Shared.DTO;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Commands.Handlers;

internal sealed class UpdateAddressHandler(IPurchaseRepository purchaseRepository, IUserContext userContext) : ICommandHandler<UpdateAddress>
{
    public async Task HandleAsync(UpdateAddress command, CancellationToken cancellationToken = default)
    {
        var customer = await purchaseRepository.GetCustomer(userContext.UserId, cancellationToken);
        if (customer is null)
            throw new CustomerNotFoundException();

        var address = customer.Addresses.FirstOrDefault(a => a.IdAddress == command.AddressDto.IdAddress);
        if (address is null)
            throw new AddressNotFoundException();

        address.UpdateAddress(
            command.AddressDto.Street,
            command.AddressDto.City,
            command.AddressDto.PostalCode,
            command.AddressDto.Country);

        await purchaseRepository.SaveCustomer(customer, cancellationToken);
    }
}