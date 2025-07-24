using AutoMapper;
using Recommendations.Purchase.Core.Data.Repositories;
using Recommendations.Purchase.Shared.DTO;
using Recommendations.Purchase.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Purchase.Core.Queries.Handlers;

internal sealed class GetCustomerOrdersHandler(IPurchaseRepository purchaseRepository, 
    IUserContext userContext, IMapper mapper) : IQueryHandler<GetCustomerOrders, IReadOnlyCollection<OrderDto>>
{
    public async Task<IReadOnlyCollection<OrderDto>> HandleAsync(GetCustomerOrders query, CancellationToken cancellationToken = default)
    {
        var orders = await purchaseRepository.GetOrders(userContext.UserId, cancellationToken);
        if(orders is null) return [];
        var customer = await purchaseRepository.GetCustomer(userContext.UserId, cancellationToken);

        var result = orders.Select(order =>
        {
            var address = customer?.Addresses.FirstOrDefault(a => a.IdAddress == order.ShippingAddressId);

            return new OrderDto(
                order.IdOrder,
                order.CustomerId,
                (int)order.Status,
                order.CreatedAt,
                order.PaidAt,
                order.Items.Select(item => new OrderItemDto(
                    item.ProductId,
                    item.ProductName,
                    item.ProductPrice,
                    item.Quantity
                )).ToList(),
                order.Payments?.Select(p => new PaymentDto(
                    p.IdPayment,
                    (uint)p.Method,
                    p.PaymentDate,
                    p.Details
                )),
                address is not null
                    ? new AddressDto(address.IdAddress, address.Street, address.City, address.PostalCode, address.Country)
                    : null!
            );
        }).ToList();

        return result;
    }
}