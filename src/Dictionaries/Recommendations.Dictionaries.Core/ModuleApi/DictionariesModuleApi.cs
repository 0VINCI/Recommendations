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

    public Task<IReadOnlyCollection<ProductDto>> GetProductsByCategory(string category)
        => queries.QueryAsync(new GetProductsByCategory(category));

    public Task<IReadOnlyCollection<ProductDto>> GetBestsellers()
        => queries.QueryAsync(new GetBestsellers());

    public Task<IReadOnlyCollection<ProductDto>> GetNewProducts()
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