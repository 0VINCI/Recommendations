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
            .Include(p => p.Details)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Images)
            .Include(p => p.Details)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyCollection<Product>> GetByIdsAsync(Guid[] ids)
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Images)
            .Include(p => p.Details)
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    // Zoptymalizowana metoda dla rekomendacji - ładuje tylko niezbędne dane
    public async Task<IReadOnlyCollection<Product>> GetByIdsForRecommendationsAsync(Guid[] ids)
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Images.Where(img => img.IsPrimary)) // Tylko główne zdjęcie
            .Where(p => ids.Contains(p.Id))
            .Select(p => new Product(
                p.Id,
                p.ExternalId,
                p.ProductDisplayName,
                p.BrandName,
                p.Price,
                p.OriginalPrice,
                p.Rating,
                p.Reviews,
                p.IsBestseller,
                p.IsNew,
                p.SubCategoryId,
                p.ArticleTypeId,
                p.BaseColourId
            ))
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> GetBestsellersAsync()
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Details)
            .Where(p => p.IsBestseller)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> GetNewProductsAsync()
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Details)
            .Where(p => p.IsNew)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> GetByCategoryAsync(string category)
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Details)
            .Where(p => p.SubCategory.Name == category || p.ArticleType.Name == category)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Product>> SearchAsync(string searchTerm)
    {
        return await context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Details)
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
            .Include(p => p.Images)
            .Include(p => p.Details)
            .AsNoTracking();
    }
    
    public async Task<Product?> GetByIdWithDetailsAsync(Guid id)
    {
        return await context.Products
            .Include(p => p.Details)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<(IReadOnlyCollection<Product> Products, int TotalCount)> GetFilteredAsync(
        string? subCategoryId,
        string? masterCategoryId,
        string? articleTypeId,
        string? baseColourId,
        decimal? minPrice,
        decimal? maxPrice,
        bool? isBestseller,
        bool? isNew,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Images)
            .Include(p => p.Details)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(subCategoryId))
            query = query.Where(p => p.SubCategory.Id == Guid.Parse(subCategoryId));

        if (!string.IsNullOrWhiteSpace(masterCategoryId))
            query = query.Where(p => p.SubCategory.MasterCategoryId == Guid.Parse(masterCategoryId));

        if (!string.IsNullOrWhiteSpace(articleTypeId))
            query = query.Where(p => p.ArticleType.Id == Guid.Parse(articleTypeId));

        if (!string.IsNullOrWhiteSpace(baseColourId))
            query = query.Where(p => p.BaseColour.Id == Guid.Parse(baseColourId));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (isBestseller.HasValue)
            query = query.Where(p => p.IsBestseller == isBestseller.Value);

        if (isNew.HasValue)
            query = query.Where(p => p.IsNew == isNew.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p =>
                p.ProductDisplayName.Contains(searchTerm) ||
                p.SubCategory.Name.Contains(searchTerm) ||
                p.ArticleType.Name.Contains(searchTerm));

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .OrderBy(p => p.ProductDisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (products, totalCount);
    }
    public async Task<(IReadOnlyCollection<Product>, int)> GetBestsellersPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Images)
            .Include(p => p.Details)
            .AsNoTracking()
            .Where(p => p.IsBestseller);

        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query.OrderByDescending(p => p.ProductDisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (products, totalCount);
    }
    public async Task<(IReadOnlyCollection<Product>, int)> GetNewProductsPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Images)
            .Include(p => p.Details)
            .AsNoTracking()
            .Where(p => p.IsNew);

        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query.OrderByDescending(p => p.ProductDisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (products, totalCount);
    }
    public async Task<(IReadOnlyCollection<Product> Products, int TotalCount)> SearchPagedAsync(
        string searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.Products
            .Include(p => p.SubCategory)
            .Include(p => p.ArticleType)
            .Include(p => p.BaseColour)
            .Include(p => p.Images)
            .Include(p => p.Details)
            .AsNoTracking()
            .Where(p =>
                p.ProductDisplayName.Contains(searchTerm) ||
                p.SubCategory.Name.Contains(searchTerm) ||
                p.ArticleType.Name.Contains(searchTerm)
            );

        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query
            .OrderBy(p => p.ProductDisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (products, totalCount);
    }
} 