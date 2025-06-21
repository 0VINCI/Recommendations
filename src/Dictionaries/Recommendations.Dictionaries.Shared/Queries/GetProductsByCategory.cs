using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Shared.Queries;

public sealed record GetProductsByCategory(string Category) : IQuery<IReadOnlyCollection<ProductDto>>; 