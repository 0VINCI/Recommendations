using Recommendations.Dictionaries.Shared;
using Recommendations.Dictionaries.Shared.Commands;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Core.ModuleApi;

internal class DictionariesModuleApi(ICommandDispatcher commands,
        IQueryDispatcher queries)
    : IDictionariesModuleApi
{
    // Product queries
    public Task<IReadOnlyCollection<ProductDto>> GetAllProducts()
        => queries.QueryAsync(new GetAllProducts());

    public Task<ProductDto?> GetProductById(Guid id)
        => queries.QueryAsync(new GetProductById(id));

    public Task<IReadOnlyCollection<ProductDto>> GetProductsByIds(Guid[] productIds)
        => queries.QueryAsync(new GetProductsByIds(productIds));

    public Task<FilteredProductDto> GetProductsByCategory(
        string? masterCategoryId = null, 
        string? subCategoryId = null, 
        int page = 1, 
        int pageSize = 20)
        => queries.QueryAsync(new GetProductsByCategory(masterCategoryId, subCategoryId, page, pageSize));

    public Task<FilteredProductDto> GetBestsellers()
        => queries.QueryAsync(new GetBestsellers());

    public Task<FilteredProductDto> GetNewProducts()
        => queries.QueryAsync(new GetNewProducts());

    public Task<IReadOnlyCollection<ProductDto>> SearchProducts(string searchTerm)
        => queries.QueryAsync(new SearchProducts(searchTerm));

    public Task AddProduct(AddProduct command)
        => commands.SendAsync(command);

    public Task UpdateProduct(UpdateProduct command)
        => commands.SendAsync(command);

    public Task DeleteProduct(DeleteProduct command)
        => commands.SendAsync(command);
}