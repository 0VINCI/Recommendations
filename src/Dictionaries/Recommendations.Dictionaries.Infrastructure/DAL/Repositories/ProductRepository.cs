using Microsoft.EntityFrameworkCore;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Infrastructure.DAL.Repositories;

internal sealed class ProductRepository(DictionariesDbContext context) : IProductRepository
{
    public async Task<IReadOnlyCollection<Product>> GetAllAsync()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyCollection<Product>> GetBestsellersAsync()
    {
        return await context.Products.Where(p => p.IsBestseller).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> GetNewProductsAsync()
    {
        return await context.Products.Where(p => p.IsNew).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> GetByCategoryAsync(string category)
    {
        return await context.Products.Where(p => p.Category == category).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> SearchAsync(string searchTerm)
    {
        return await context.Products
            .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
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
} 