using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Dictionaries.Shared.Commands;

public sealed record DeleteProduct(Guid Id) : ICommand; 