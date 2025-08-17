using Recommendations.ContentBased.Shared.DTO;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.ContentBased.Shared.Commands;

public record CreateProductEmbedding(CreateProductEmbeddingDto ProductEmbedding) : ICommand;
