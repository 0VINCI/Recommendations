using Recommendations.Authorization.Shared.Commands;
using Recommendations.Authorization.Shared.DTO;

namespace Recommendations.Authorization.Shared;

public interface IAuthorizationModuleApi
{
    Task SignIn(SignIn command);
    Task SignUp(SignUp command);
    Task ChangePassword(ChangePassword command);
    Task RemindPassword(RemindPassword command);
    Task <IReadOnlyCollection<UserDto>> GetAllUsers();
}