using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recommendations.Shared.Abstractions.Email;
using Recommendations.Shared.Infrastructure.Exceptions;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.Shared.Infrastructure.Email;

public class SendEmailService(HttpClient httpClient,
    IOptions<EmailOptions> options) : ISendEmailService
{
    private readonly EmailOptions _options = options.Value;

    public async Task SendEmailAsync(string to, string name, string subject, string body)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _options.Url);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiToken);

        var payload = new
        {
            from = new { email = _options.Email, name },
            to = new[] { new { email = to } },
            subject,
            html = body
        };

        request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new FailedSendEmailException(content);
        }
    }
}