using Pgvector;

namespace Recommendations.VisualBased.Core.Types;

public class ItemVisual
{
    public string ItemId { get; set; } = default!;
    public Vector Emb { get; set; } = default!;
    public string ModelVer { get; set; } = default!;
    public DateTimeOffset GeneratedAt { get; set; }

}