using AutoMapper;
using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Shared.DTO;
using Recommendations.Purchase.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Queries.Handlers;

internal sealed class GetCustomerOrdersHandler(IPurchaseRepository purchaseRepository, 
    IUserContext userContext) : IQueryHandler<GetCustomerOrders, IReadOnlyCollection<OrderDto>?>
{
    public async Task<IReadOnlyCollection<OrderDto>?> HandleAsync(GetCustomerOrders query,
        CancellationToken cancellationToken = default)
    {
        var orders = await purchaseRepository.GetOrders(userContext.UserId, cancellationToken);
        
        return orders;
    }
}