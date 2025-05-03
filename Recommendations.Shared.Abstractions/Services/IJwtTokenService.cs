namespace Recommendations.Shared.Abstractions.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string email);
    }
}