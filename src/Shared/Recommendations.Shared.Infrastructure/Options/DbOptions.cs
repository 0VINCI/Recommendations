using Microsoft.Extensions.Options;

namespace Recommendations.Shared.Infrastructure.Options;

public record DbOptions
{
    public const string SectionName = "DatabaseConnections:AuthorizationDbOptions";

    public string DatabaseConnection { get; init; } = default!;
}