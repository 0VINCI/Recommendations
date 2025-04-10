namespace Recommendations.Shared.Infrastructure.Email;

public interface ISendEmailService
{
    public Task SendEmailAsync(string to, string subject, string body);
}