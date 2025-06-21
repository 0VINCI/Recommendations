using Recommendations.Purchase.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Purchase.Shared.Queries;

public sealed record GetOrderById(Guid OrderId) : IQuery<OrderDto?>;