namespace Recommendations.Shared.Infrastructure.Options;

public record JwtOptions
{
    public const string SectionName = "JwtToken";

    public string Key      { get; init; } = default!;
    public string Issuer      { get; init; } = default!;
    public string Audience      { get; init; } = default!;
    public string ExpiresInMinutes    { get; init; } = default!;
}
