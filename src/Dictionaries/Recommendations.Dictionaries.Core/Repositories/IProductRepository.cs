using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Core.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
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
} 