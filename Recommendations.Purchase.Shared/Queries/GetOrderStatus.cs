using Recommendations.Purchase.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Purchase.Shared.Queries;

public sealed record GetOrderStatus(Guid OrderId) : IQuery<int?>;