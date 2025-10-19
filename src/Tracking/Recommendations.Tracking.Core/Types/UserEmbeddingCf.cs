using Pgvector;

namespace Recommendations.Tracking.Core.Types;

public sealed class UserEmbeddingCf
{
    public string UserKey { get; set; } = default!;
    public Vector Emb { get; set; } = default!;
    public string ModelVer { get; set; } = default!;
    public DateTimeOffset TrainedAt { get; set; }
}