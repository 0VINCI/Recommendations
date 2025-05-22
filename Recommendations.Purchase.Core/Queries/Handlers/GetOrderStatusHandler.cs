using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Shared.DTO;
using Recommendations.Purchase.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Queries.Handlers;

internal sealed class GetOrderStatusHandler(IPurchaseRepository purchaseRepository, 
    IUserContext userContext) : IQueryHandler<GetOrderStatus, int?>
{
    public async Task<int?> HandleAsync(GetOrderStatus query,
        CancellationToken cancellationToken = default)
    {
        var order = await purchaseRepository.GetOrderStatus(query.OrderId, cancellationToken);
        
        return order;
    }
}