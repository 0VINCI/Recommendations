namespace Recommendations.Shared.Abstractions.Email;

public interface ISendEmailService
{
    public Task SendEmailAsync(string to, string name, string subject, string body);
}