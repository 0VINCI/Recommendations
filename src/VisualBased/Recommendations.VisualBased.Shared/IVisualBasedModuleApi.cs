
using Recommendations.Dictionaries.Shared.DTO;

namespace Recommendations.VisualBased.Shared;

public interface IVisualBasedModuleApi
{
    Task<IEnumerable<ProductDto>> GetSimilarProducts(Guid productId, int topCount, CancellationToken cancellationToken = default);
}