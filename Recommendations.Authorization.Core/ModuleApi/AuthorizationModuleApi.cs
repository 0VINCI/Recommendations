using Recommendations.Authorization.Shared;
using Recommendations.Authorization.Shared.Commands;
using Recommendations.Authorization.Shared.DTO;
using Recommendations.Authorization.Shared.Queries;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Core.ModuleApi;
internal class AuthorizationModuleApi(ICommandDispatcher commands,
        IQueryDispatcher queries)
    : IAuthorizationModuleApi
{
    public Task<SignedInDto> SignIn(SignInDto signInDto)
        => queries.QueryAsync(new SignIn(signInDto));

    public Task SignUp(SignUp cmd) 
        => commands.SendAsync(cmd);

    public Task ChangePassword(ChangePassword cmd) 
        => commands.SendAsync(cmd);

    public Task RemindPassword(RemindPassword cmd) 
        => commands.SendAsync(cmd);

    public Task<IReadOnlyCollection<UserDto>> GetAllUsers() 
        => queries.QueryAsync(new GetAllUsers());
}
