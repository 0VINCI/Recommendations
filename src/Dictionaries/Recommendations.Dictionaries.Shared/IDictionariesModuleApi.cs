using Recommendations.Dictionaries.Shared.Commands;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;

namespace Recommendations.Dictionaries.Shared;

public interface IDictionariesModuleApi
{
    Task<IReadOnlyCollection<ProductDto>> GetAllProducts();
    Task<ProductDto?> GetProductById(Guid id);
    Task<IReadOnlyCollection<ProductDto>> GetProductsByCategory(string category);
    Task<IReadOnlyCollection<ProductDto>> GetBestsellers();
    Task<IReadOnlyCollection<ProductDto>> GetNewProducts();
    Task<IReadOnlyCollection<ProductDto>> SearchProducts(string searchTerm);
    
    Task AddProduct(AddProduct command);
    Task UpdateProduct(UpdateProduct command);
    Task DeleteProduct(DeleteProduct command);
}