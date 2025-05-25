using AutoMapper;
using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Shared.DTO;
using Recommendations.Purchase.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Queries.Handlers;

internal sealed class GetCustomerHandler(IPurchaseRepository purchaseRepository, 
    IUserContext userContext, IMapper mapper) : IQueryHandler<GetCustomer, CustomerDto?>
{
    public async Task<CustomerDto?> HandleAsync(GetCustomer query,
        CancellationToken cancellationToken = default)
    {
        var customer = await purchaseRepository.GetCustomer(userContext.UserId, cancellationToken);
        
        return customer is null ? null : mapper.Map<CustomerDto?>(customer);
    }
}