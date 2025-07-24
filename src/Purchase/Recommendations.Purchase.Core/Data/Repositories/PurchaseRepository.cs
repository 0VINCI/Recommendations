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
            .AsNoTracking()
            .Include(o => o.Items)
            .Where(o => o.CustomerId == customer.IdCustomer)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyCollection<Order>>(orders);
    }
    public async Task<Customer?> GetCustomer(Guid userId, CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        
        return customer is null ? null : mapper.Map<Customer?>(customer);
    }
    
    public async Task<Order?> GetOrderById(Guid orderId, CancellationToken cancellationToken = default)
    {
        var orders = await dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdOrder == orderId, cancellationToken);
        
        return mapper.Map<Order>(orders);
    }
    
    public async Task<OrderStatusDto[]> GetOrdersStatusById(Guid[] orderIds, CancellationToken cancellationToken = default)
    {
        var orders = await dbContext.Orders
            .AsNoTracking()
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
        var dbModel = await dbContext.Orders
            .Include(o => o.Items)
            .Include(o => o.Payments)
            .FirstOrDefaultAsync(o => o.IdOrder == order.IdOrder, cancellationToken);

        if (dbModel == null)
            throw new InvalidOperationException("Order not found in DB");

        dbModel.Status = order.Status;
        dbModel.PaidAt = order.PaidAt;
        dbModel.ShippingAddressId = order.ShippingAddressId;

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
        var dbModel = await dbContext.Customers
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.IdCustomer == customer.IdCustomer, cancellationToken);

        if (dbModel == null)
            throw new InvalidOperationException("Customer not found in DB");

        dbModel.FirstName = customer.FirstName;
        dbModel.LastName = customer.LastName;
        dbModel.Email = customer.Email;
        dbModel.PhoneNumber = customer.PhoneNumber;

        foreach (var address in customer.Addresses)
        {
            var dbAddress = dbModel.Addresses.FirstOrDefault(a => a.IdAddress == address.IdAddress);
            if (dbAddress == null)
            {
                dbModel.Addresses.Add(new AddressDbModel
                {
                    IdAddress = address.IdAddress,
                    Street = address.Street,
                    City = address.City,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                });
            }
            else
            {
                dbAddress.Street = address.Street;
                dbAddress.City = address.City;
                dbAddress.PostalCode = address.PostalCode;
                dbAddress.Country = address.Country;
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}