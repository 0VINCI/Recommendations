using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Dictionaries.Shared.Commands;

public sealed record UpdateProduct(ProductDto Product) : ICommand; 