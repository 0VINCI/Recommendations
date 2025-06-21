using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Shared.Queries;

public sealed record SearchProducts(string SearchTerm) : IQuery<IReadOnlyCollection<ProductDto>>; 