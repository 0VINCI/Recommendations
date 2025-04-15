using Recommendations.Authorization.Core.Types;

namespace Recommendations.Authorization.Infrastructure.DAL.Repositories;

public interface IUserRepository
{
    public Task<User> GetUser(string email);
    public Task<IReadOnlyCollection<User>> GetAllUsers();
    public Task AddAsync(User user);
    public Task Update(User user);
    public Task Remove(string email);
    Task<bool> ExistsByEmailAsync(string email);
}