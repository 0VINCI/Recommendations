using Recommendations.ContentBased.Shared.DTO;
using Recommendations.ContentBased.Shared.Types;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.ContentBased.Shared.Commands;

public record UpdateProductEmbedding(Guid ProductId, VectorType Variant, UpdateProductEmbeddingDto ProductEmbedding) : ICommand;
