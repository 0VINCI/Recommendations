using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recommendations.Purchase.Core.Data.Models;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Shared.DTO;

namespace Recommendations.Purchase.Core.Data.Repositories;

internal sealed class PurchaseRepository(PurchaseDbContext dbContext, IMapper mapper) : IPurchaseRepository
{
    public async Task<IReadOnlyCollection<OrderDto>?> GetOrders(Guid userId, CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (customer is null)
            return null;

        var orders = await dbContext.Orders
            .Include(o => o.Items)
            .Where(o => o.CustomerId == customer.IdCustomer)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyCollection<OrderDto>>(orders);
    }
    public async Task<CustomerDto?> GetCustomer(Guid userId, CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        
        return customer is null ? null : mapper.Map<CustomerDto?>(customer);
    }
    
    public async Task<OrderDto?> GetOrderById(Guid orderId, CancellationToken cancellationToken = default)
    {
        var orders = await dbContext.Orders
            .FirstOrDefaultAsync(c => c.IdOrder == orderId, cancellationToken);
        
        return mapper.Map<OrderDto>(orders);
    }
    
    public async Task<int?> GetOrderStatus(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .FirstOrDefaultAsync(c => c.IdOrder == orderId, cancellationToken);

        return order is null ? null : mapper.Map<int?>(order.Status);
    }
    public async Task AddNewOrder(Order order, CancellationToken cancellationToken = default)
    {
        var dbModel = mapper.Map<OrderDbModel>(order);
        dbContext.Orders.Add(dbModel);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}