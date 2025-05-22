using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Shared.DTO;
using Recommendations.Purchase.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Queries.Handlers;

internal sealed class GetOrderByIdHandler(IPurchaseRepository purchaseRepository, 
    IUserContext userContext) : IQueryHandler<GetOrderById, OrderDto?>
{
    public async Task<OrderDto?> HandleAsync(GetOrderById query,
        CancellationToken cancellationToken = default)
    {
        var order = await purchaseRepository.GetOrderById(userContext.UserId, cancellationToken);
        
        return order;
    }
}