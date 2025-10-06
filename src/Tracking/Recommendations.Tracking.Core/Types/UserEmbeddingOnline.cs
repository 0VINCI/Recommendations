namespace Recommendations.Tracking.Core.Types;

public sealed class UserEmbeddingOnline
{
    public string UserKey { get; set; } = default!;
    public float[] Emb { get; set; } = default!;      // pgvector
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}