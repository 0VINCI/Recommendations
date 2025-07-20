using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Repositories;

internal sealed class SubCategoryRepository(DictionariesDbContext context) : ISubCategoryRepository
{
    public async Task<IReadOnlyCollection<SubCategory>> GetAllAsync()
    {
        return await context.SubCategories.ToListAsync();
    }

    public async Task<SubCategory?> GetByIdAsync(Guid id)
    {
        return await context.SubCategories.FirstOrDefaultAsync(sc => sc.Id == id);
    }

    public async Task<SubCategory?> GetByNameAsync(string name)
    {
        return await context.SubCategories.FirstOrDefaultAsync(sc => sc.Name == name);
    }

    public async Task AddAsync(SubCategory subCategory)
    {
        await context.SubCategories.AddAsync(subCategory);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SubCategory subCategory)
    {
        context.SubCategories.Update(subCategory);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var subCategory = await context.SubCategories.FirstOrDefaultAsync(sc => sc.Id == id);
        if (subCategory != null)
        {
            context.SubCategories.Remove(subCategory);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.SubCategories.AnyAsync(sc => sc.Id == id);
    }
    public async Task<IReadOnlyCollection<SubCategory>> GetByMasterCategoryIdAsync(Guid masterCategoryId)
    {
        return await context.SubCategories
            .Where(sc => sc.MasterCategoryId == masterCategoryId)
            .ToListAsync();
    }
} 