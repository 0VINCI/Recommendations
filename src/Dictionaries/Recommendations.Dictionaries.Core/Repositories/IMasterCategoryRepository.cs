using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Core.Repositories;

public interface IMasterCategoryRepository
{
    Task<IReadOnlyCollection<MasterCategory>> GetAllAsync();
    Task<MasterCategory?> GetByIdAsync(Guid id);
    Task<MasterCategory?> GetByNameAsync(string name);
    Task AddAsync(MasterCategory masterCategory);
    Task UpdateAsync(MasterCategory masterCategory);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}