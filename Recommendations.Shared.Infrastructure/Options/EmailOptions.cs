namespace Recommendations.Shared.Infrastructure.Options;

public record EmailOptions
{
    public const string SectionName = "SendEmail";

    public string ApiToken      { get; init; } = default!;
    public string Url      { get; init; } = default!;
    public string Email    { get; init; } = default!;
}
