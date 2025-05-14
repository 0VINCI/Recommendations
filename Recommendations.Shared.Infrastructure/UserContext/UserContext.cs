using Microsoft.AspNetCore.Http;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Shared.Infrastructure.UserContext;

public class UserContext(IHttpContextAccessor http) : IUserContext
{
    public Guid UserId =>
        Guid.Parse(http.HttpContext!.User.FindFirst("sub")!.Value);
}