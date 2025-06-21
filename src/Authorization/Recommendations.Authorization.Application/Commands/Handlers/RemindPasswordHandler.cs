using Recommendations.Authorization.Application.Exceptions;
using Recommendations.Authorization.Infrastructure.DAL.Repositories;
using Recommendations.Authorization.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Email;

namespace Recommendations.Authorization.Application.Commands.Handlers;

internal sealed class RemindPasswordHandler(IUserRepository userRepository,
    ISendEmailService emailService) : ICommandHandler<RemindPassword>
{
    public async Task HandleAsync(RemindPassword command, CancellationToken cancellationToken = default)
    {
        var data = command.Email;

        var user = await userRepository.GetUser(data);

        if (user is null)
        {
            throw new UserNotFoundException();
        }
        
        var resetCode = GenerateResetCode();
        user.ChangePassword(resetCode);

        const string name = "Password reminder";
        const string subject = "Password Reset Code";
        var body = $"Your password reset code is: {resetCode}";
        
        await userRepository.Update(user);
        await emailService.SendEmailAsync(user.Email, name, subject, body);
    }
    
    private static string GenerateResetCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

}