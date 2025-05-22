using Recommendations.Purchase.Shared.DTO;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record CreateOrder(OrderDto OrderDto) : ICommand;