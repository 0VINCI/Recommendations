using Recommendations.Dictionaries.Shared.Commands;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;

namespace Recommendations.Dictionaries.Shared;

public interface IDictionariesModuleApi
{
    Task<IReadOnlyCollection<ProductDto>> GetAllProducts();
    Task<ProductDto?> GetProductById(Guid id);
    Task<IReadOnlyCollection<ProductDto>> GetProductsByIds(Guid[] productIds);
    Task<IReadOnlyCollection<ProductDto>> GetProductsByIdsForRecommendations(Guid[] productIds);
    Task<FilteredProductDto> GetProductsByCategory(
        string? masterCategoryId = null,
        string? subCategoryId = null,
        int page = 1,
        int pageSize = 20);
    Task<FilteredProductDto> GetBestsellers();
    Task<FilteredProductDto> GetNewProducts();
    Task<IReadOnlyCollection<ProductDto>> SearchProducts(string searchTerm);
    
    Task AddProduct(AddProduct command);
    Task UpdateProduct(UpdateProduct command);
    Task DeleteProduct(DeleteProduct command);
}