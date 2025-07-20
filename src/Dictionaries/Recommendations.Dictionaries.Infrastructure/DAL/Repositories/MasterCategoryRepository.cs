using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Repositories;

internal sealed class MasterCategoryRepository(DictionariesDbContext context) : IMasterCategoryRepository
{
    public async Task<IReadOnlyCollection<MasterCategory>> GetAllAsync()
    {
        return await context.MasterCategories
            .Include(mc => mc.SubCategories)
            .ToListAsync();
    }

    public async Task<MasterCategory?> GetByIdAsync(Guid id)
    {
        return await context.MasterCategories
            .Include(mc => mc.SubCategories)
            .FirstOrDefaultAsync(mc => mc.Id == id);
    }

    public async Task<MasterCategory?> GetByNameAsync(string name)
    {
        return await context.MasterCategories
            .Include(mc => mc.SubCategories)
            .FirstOrDefaultAsync(mc => mc.Name == name);
    }

    public async Task AddAsync(MasterCategory masterCategory)
    {
        await context.MasterCategories.AddAsync(masterCategory);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MasterCategory masterCategory)
    {
        context.MasterCategories.Update(masterCategory);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var masterCategory = await context.MasterCategories.FirstOrDefaultAsync(mc => mc.Id == id);
        if (masterCategory != null)
        {
            context.MasterCategories.Remove(masterCategory);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.MasterCategories.AnyAsync(mc => mc.Id == id);
    }
}