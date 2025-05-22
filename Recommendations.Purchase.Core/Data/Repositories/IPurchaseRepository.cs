using Recommendations.Purchase.Core.Data.EF;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Shared.DTO;

namespace Recommendations.Purchase.Core.Data.Repositories;

internal interface IPurchaseRepository
{
    Task<IReadOnlyCollection<OrderDto>?> GetOrders(Guid userId, CancellationToken cancellationToken = default);
    Task<CustomerDto?> GetCustomer(Guid userId, CancellationToken cancellationToken = default);
    Task<OrderDto?> GetOrderById(Guid orderId, CancellationToken cancellationToken = default);
    Task<int?> GetOrderStatus(Guid orderId, CancellationToken cancellationToken = default);
    Task AddNewOrder(Order order, CancellationToken cancellationToken = default);
}