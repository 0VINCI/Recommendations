using NpgsqlTypes;

namespace Recommendations.Tracking.Core.Types;

public enum EventSource 
{ 
    [PgName("frontend")] Frontend = 0,
    [PgName("backend")] Backend = 1
}
