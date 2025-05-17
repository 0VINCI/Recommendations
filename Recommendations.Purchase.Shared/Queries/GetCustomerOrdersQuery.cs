using Recommendations.Purchase.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Purchase.Shared.Queries;

public sealed record GetCustomerOrdersQuery() : IQuery<IReadOnlyCollection<OrderDto>>;