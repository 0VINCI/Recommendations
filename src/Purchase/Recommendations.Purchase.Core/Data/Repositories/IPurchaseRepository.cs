using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Shared.DTO;

namespace Recommendations.Purchase.Core.Data.Repositories;

internal interface IPurchaseRepository
{
    Task<IReadOnlyCollection<Order>?> GetOrders(Guid userId, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomer(Guid userId, CancellationToken cancellationToken = default);
    Task<Order?> GetOrderById(Guid orderId, CancellationToken cancellationToken = default);
    Task<OrderStatusDto[]> GetOrdersStatusById(Guid[] orderId, CancellationToken cancellationToken = default);
    Task AddNewOrder(Order order, CancellationToken cancellationToken = default);
    Task SaveOrder(Order order, CancellationToken cancellationToken = default);
    Task AddNewCustomer(Customer customer, CancellationToken cancellationToken = default);
    Task SaveCustomer(Customer customer, CancellationToken cancellationToken = default);
}