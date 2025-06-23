using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Repositories;

internal sealed class BaseColourRepository(DictionariesDbContext context) : IBaseColourRepository
{
    public async Task<IReadOnlyCollection<BaseColour>> GetAllAsync()
    {
        return await context.BaseColours.ToListAsync();
    }

    public async Task<BaseColour?> GetByIdAsync(Guid id)
    {
        return await context.BaseColours.FirstOrDefaultAsync(bc => bc.Id == id);
    }

    public async Task<BaseColour?> GetByNameAsync(string name)
    {
        return await context.BaseColours.FirstOrDefaultAsync(bc => bc.Name == name);
    }

    public async Task AddAsync(BaseColour baseColour)
    {
        await context.BaseColours.AddAsync(baseColour);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BaseColour baseColour)
    {
        context.BaseColours.Update(baseColour);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var baseColour = await context.BaseColours.FirstOrDefaultAsync(bc => bc.Id == id);
        if (baseColour != null)
        {
            context.BaseColours.Remove(baseColour);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.BaseColours.AnyAsync(bc => bc.Id == id);
    }
} 