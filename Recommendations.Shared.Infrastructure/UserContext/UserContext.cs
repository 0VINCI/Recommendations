using Microsoft.AspNetCore.Http;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Shared.Infrastructure.UserContext;

public class UserContext(IHttpContextAccessor http) : IUserContext
{
    public Guid UserId
    {
        get
        {
            var claims = http.HttpContext?.User?.Claims?.ToList();
            if (claims is null || claims.Count == 0)
                throw new InvalidOperationException("User has no claims.");
            
            var claim = claims.FirstOrDefault(c => c.Type is "sub" or System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim is null)
                throw new InvalidOperationException("JWT token does not contain 'sub' or NameIdentifier claim.");

            return Guid.Parse(claim.Value);
        }
    }
}
