using Microsoft.Extensions.DependencyInjection;
using Recommendations.Purchase.Core.Queries.Handlers;
using Recommendations.Purchase.Shared.DTO;
using Recommendations.Purchase.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Purchase.Core.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetCustomer, CustomerDto?>, GetCustomerHandler>();
        services.AddScoped<IQueryHandler<GetCustomerOrders, IReadOnlyCollection<OrderDto>?>, GetCustomerOrdersHandler>();
        services.AddScoped<IQueryHandler<GetOrderById, OrderDto?>, GetOrderByIdHandler>();
        services.AddScoped<IQueryHandler<GetOrdersStatus, OrderStatusDto[]>, GetOrdersStatusHandler>();
        
        return services;
    }
}