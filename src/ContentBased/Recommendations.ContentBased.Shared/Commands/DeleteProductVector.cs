using Recommendations.ContentBased.Shared.Types;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.ContentBased.Shared.Commands;

public record DeleteProductEmbedding(Guid ProductId, VectorType Variant) : ICommand;
public record DeleteProductEmbeddings(Guid ProductId) : ICommand;
