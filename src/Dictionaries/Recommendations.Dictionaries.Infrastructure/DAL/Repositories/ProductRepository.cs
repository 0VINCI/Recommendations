using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Repositories;

internal sealed class ProductRepository(DictionariesDbContext context) : IProductRepository
{
    public async Task<IReadOnlyCollection<Product>> GetAllAsync()
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyCollection<Product>> GetBestsellersAsync()
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Where(p => p.IsBestseller)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> GetNewProductsAsync()
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Where(p => p.IsNew)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> GetByCategoryAsync(string category)
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Where(p => p.SubCategory.Name == category || p.ArticleType.Name == category)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> SearchAsync(string searchTerm)
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Where(p => p.ProductDisplayName.Contains(searchTerm) || 
                       p.SubCategory.Name.Contains(searchTerm) ||
                       p.ArticleType.Name.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product != null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Products.AnyAsync(p => p.Id == id);
    }
    
    public IQueryable<Product> AsQueryable()
    {
        return context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .AsNoTracking();
    }
    
    public async Task<Product?> GetByIdWithDetailsAsync(Guid id)
    {
        return await context.Products
            .Include(p => p.Details)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
} 