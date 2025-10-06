namespace Recommendations.Tracking.Core.Types;

public sealed class UserItemInteraction
{
    public string UserKey { get; set; } = default!;   // user_id lub "anon:<uuid>"
    public string ItemId { get; set; } = default!;
    public int Views { get; set; }
    public int Clicks { get; set; }
    public int Carts { get; set; }
    public int Purchases { get; set; }
    public float Weight { get; set; }
    public DateTimeOffset LastTs { get; set; }
}