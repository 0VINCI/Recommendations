using Recommendations.Purchase.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Purchase.Shared.Queries;

public sealed record GetOrdersStatus(Guid[] OrderIds) : IQuery<OrderStatusDto[]>;