namespace Recommendations.Tracking.Core.Types;

public sealed class ItemEmbeddingCf
{
    public string ItemId { get; set; } = default!;
    public float[] Emb { get; set; } = default!;
    public string ModelVer { get; set; } = default!;
    public DateTimeOffset TrainedAt { get; set; }
}
