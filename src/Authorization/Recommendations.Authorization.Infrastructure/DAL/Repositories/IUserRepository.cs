using Recommendations.Authorization.Core.Types;

namespace Recommendations.Authorization.Infrastructure.DAL.Repositories;

public interface IUserRepository
{
    public Task<User> GetUserByEmail(string email);
    public Task<User> GetUserById(Guid userId);
    public Task<IReadOnlyCollection<User>> GetAllUsers();
    public Task AddAsync(User user);
    public Task Update(User user);
    public Task Remove(string email);
    Task<bool> ExistsByEmailAsync(string email);
}