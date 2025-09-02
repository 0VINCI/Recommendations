using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Shared.Queries;

public sealed record GetSubCategories(Guid? MasterCategoryId = null) : IQuery<IReadOnlyCollection<SubCategoryDto>>;
