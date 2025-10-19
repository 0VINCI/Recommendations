using Pgvector;

namespace Recommendations.Tracking.Core.Types;

public sealed class UserEmbeddingOnline
{
    public string UserKey { get; set; } = default!;
    public Vector Emb { get; set; } = default!;      // pgvector
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}