using Microsoft.Extensions.Options;

namespace Recommendations.Shared.Infrastructure.Options;

public class DbOptions
{
    public const string SectionName = "DatabaseConnections:AuthorizationDbOptions";

    public string DatabaseConnection { get; set; }
}