using AutoMapper;
using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Shared.DTO;
using Recommendations.Purchase.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Purchase.Core.Queries.Handlers;

internal sealed class GetOrderByIdHandler(IPurchaseRepository purchaseRepository, 
    IMapper mapper) : IQueryHandler<GetOrderById, OrderDto?>
{
    public async Task<OrderDto?> HandleAsync(GetOrderById query,
        CancellationToken cancellationToken = default)
    {
        var order = await purchaseRepository.GetOrderById(query.OrderId, cancellationToken);
        
        return order is null ? null : mapper.Map<OrderDto?>(order);
    }
}