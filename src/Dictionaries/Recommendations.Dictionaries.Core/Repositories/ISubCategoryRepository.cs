using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Core.Repositories;

public interface ISubCategoryRepository
{
    Task<SubCategory?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<SubCategory>> GetAllAsync();
    Task<SubCategory?> GetByNameAsync(string name);
    Task AddAsync(SubCategory subCategory);
    Task UpdateAsync(SubCategory subCategory);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<IReadOnlyCollection<SubCategory>> GetByMasterCategoryIdAsync(Guid masterCategoryId);

} 