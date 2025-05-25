using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recommendations.Purchase.Core.Data.Models;
using Recommendations.Purchase.Core.Types;
using Recommendations.Purchase.Shared.DTO;

namespace Recommendations.Purchase.Core.Data.Repositories;

internal sealed class PurchaseRepository(PurchaseDbContext dbContext, IMapper mapper) : IPurchaseRepository
{
    public async Task<IReadOnlyCollection<Order>?> GetOrders(Guid userId, CancellationToken cancellationToken = default)
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

        return mapper.Map<IReadOnlyCollection<Order>>(orders);
    }
    public async Task<Customer?> GetCustomer(Guid userId, CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        
        return customer is null ? null : mapper.Map<Customer?>(customer);
    }
    
    public async Task<Order?> GetOrderById(Guid orderId, CancellationToken cancellationToken = default)
    {
        var orders = await dbContext.Orders
            .FirstOrDefaultAsync(c => c.IdOrder == orderId, cancellationToken);
        
        return mapper.Map<Order>(orders);
    }
    
    public async Task<OrderStatusDto[]> GetOrdersStatusById(Guid[] orderIds, CancellationToken cancellationToken = default)
    {
        var orders = await dbContext.Orders
            .Where(o => orderIds.Contains(o.IdOrder))
            .ToListAsync(cancellationToken);

        return orders
            .Select(o => new OrderStatusDto(o.IdOrder, (int)o.Status))
            .ToArray();
    }
    public async Task AddNewOrder(Order order, CancellationToken cancellationToken = default)
    {
        var dbModel = mapper.Map<OrderDbModel>(order);
        dbContext.Orders.Add(dbModel);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task SaveOrder(Order order, CancellationToken cancellationToken = default)
    {
        var dbModel = mapper.Map<OrderDbModel>(order);
        dbContext.Orders.Update(dbModel);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddNewCustomer(Customer customer, CancellationToken cancellationToken = default)
    {
        var dbModel = mapper.Map<CustomerDbModel>(customer);
        dbContext.Customers.Add(dbModel);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveCustomer(Customer customer, CancellationToken cancellationToken = default)
    {
        var dbModel = mapper.Map<CustomerDbModel>(customer);
        dbContext.Customers.Update(dbModel);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}