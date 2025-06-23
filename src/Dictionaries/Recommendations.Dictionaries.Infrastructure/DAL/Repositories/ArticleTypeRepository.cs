using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Repositories;

internal sealed class ArticleTypeRepository(DictionariesDbContext context) : IArticleTypeRepository
{
    public async Task<IReadOnlyCollection<ArticleType>> GetAllAsync()
    {
        return await context.ArticleTypes
            .Include(at => at.SubCategory)
            .ToListAsync();
    }

    public async Task<ArticleType?> GetByIdAsync(Guid id)
    {
        return await context.ArticleTypes
            .Include(at => at.SubCategory)
            .FirstOrDefaultAsync(at => at.Id == id);
    }

    public async Task<ArticleType?> GetByNameAsync(string name)
    {
        return await context.ArticleTypes
            .Include(at => at.SubCategory)
            .FirstOrDefaultAsync(at => at.Name == name);
    }

    public async Task<IReadOnlyCollection<ArticleType>> GetBySubCategoryAsync(Guid subCategoryId)
    {
        return await context.ArticleTypes
            .Include(at => at.SubCategory)
            .Where(at => at.SubCategoryId == subCategoryId)
            .ToListAsync();
    }

    public async Task AddAsync(ArticleType articleType)
    {
        await context.ArticleTypes.AddAsync(articleType);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ArticleType articleType)
    {
        context.ArticleTypes.Update(articleType);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var articleType = await context.ArticleTypes.FirstOrDefaultAsync(at => at.Id == id);
        if (articleType != null)
        {
            context.ArticleTypes.Remove(articleType);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.ArticleTypes.AnyAsync(at => at.Id == id);
    }
} 