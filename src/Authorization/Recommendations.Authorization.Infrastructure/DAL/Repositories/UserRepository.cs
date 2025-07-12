using Microsoft.EntityFrameworkCore;
using Recommendations.Authorization.Core.Exceptions;
using Recommendations.Authorization.Core.Types;

namespace Recommendations.Authorization.Infrastructure.DAL.Repositories;

internal sealed class UserRepository(AuthorizationDbContext dbContext) : IUserRepository
{
    public async Task<User> GetUser(string email)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email.Equals(email));
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }
    
    public async Task<IReadOnlyCollection<User>> GetAllUsers()
    {
        var users = await dbContext.Users.ToListAsync();
        if (users.Count == 0)
        {
            throw new UserNotFoundException();
        }

        return users;
    }

    public async Task AddAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task Update(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task Remove(string email)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email.Equals(email));
        
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await dbContext.Users.AnyAsync(x => x.Email.Equals(email));
    }

}