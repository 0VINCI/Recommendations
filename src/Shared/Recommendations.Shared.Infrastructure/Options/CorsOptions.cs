using Microsoft.Extensions.Options;

namespace Recommendations.Shared.Infrastructure.Options;

public sealed record CorsOptions(string[] Urls, string[] Methods, string[] Headers)
{
    public static string SectionName => "Cors";
}
