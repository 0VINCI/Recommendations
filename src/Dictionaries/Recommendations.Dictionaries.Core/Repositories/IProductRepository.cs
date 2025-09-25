using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Core.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Product>> GetByIdsAsync(Guid[] ids);
    Task<IReadOnlyCollection<Product>> GetAllAsync();
    Task<IReadOnlyCollection<Product>> GetByCategoryAsync(string category);
    Task<IReadOnlyCollection<Product>> GetBestsellersAsync();
    Task<IReadOnlyCollection<Product>> GetNewProductsAsync();
    Task<IReadOnlyCollection<Product>> SearchAsync(string searchTerm);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<Product?> GetByIdWithDetailsAsync(Guid id);
    IQueryable<Product> AsQueryable();
    Task<(IReadOnlyCollection<Product> Products, int TotalCount)> GetFilteredAsync(
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
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyCollection<Product> Products, int TotalCount)> GetBestsellersPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyCollection<Product> Products, int TotalCount)> GetNewProductsPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<(IReadOnlyCollection<Product> Products, int TotalCount)> SearchPagedAsync(
        string searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
} 