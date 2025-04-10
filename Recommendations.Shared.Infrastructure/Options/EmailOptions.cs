namespace Recommendations.Shared.Infrastructure.Options;

public class EmailOptions
{
    public const string SectionName = "SendEmail";

    public string ApiToken { get; }
    public string Url { get; }
    public string Email { get; }
    public string Name { get; }
}