using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Recommendations.Shared.Abstractions.Services;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.Shared.Infrastructure.Services;

public class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    public string GenerateToken(string userId, string email)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var opt = options.Value;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: opt.Issuer,
            audience: opt.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(opt.ExpiresInMinutes)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}