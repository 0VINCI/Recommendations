using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Shared.DTO;
using Recommendations.Purchase.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Purchase.Core.Queries.Handlers;

internal sealed class GetOrdersStatusHandler(IPurchaseRepository purchaseRepository) : IQueryHandler<GetOrdersStatus, OrderStatusDto[]>
{
    public async Task<OrderStatusDto[]> HandleAsync(GetOrdersStatus query,
        CancellationToken cancellationToken = default)
    {
        var statuses = await purchaseRepository.GetOrdersStatusById(query.OrderIds, cancellationToken);
        
        return statuses;
    }
}