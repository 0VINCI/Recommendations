using Microsoft.Extensions.DependencyInjection;
using Recommendations.Purchase.Core.Commands.Handlers;
using Recommendations.Purchase.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Core.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<AddNewAddress>, AddNewAddressHandler>();
        services.AddScoped<ICommandHandler<AddNewCustomer>, AddNewCustomerHandler>();
        services.AddScoped<ICommandHandler<CreateOrder>, CreateOrderHandler>();
        services.AddScoped<ICommandHandler<PayForOrder>, PayForOrderHandler>();
        services.AddScoped<ICommandHandler<UpdateAddress>, UpdateAddressHandler>();
        services.AddScoped<ICommandHandler<UpdateCustomer>, UpdateCustomerHandler>();
        services.AddScoped<ICommandHandler<UpdateStatus>, UpdateStatusHandler>();

        return services;
    }
}