using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Core.Repositories;

public interface IBaseColourRepository
{
    Task<BaseColour?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<BaseColour>> GetAllAsync();
    Task<BaseColour?> GetByNameAsync(string name);
    Task AddAsync(BaseColour baseColour);
    Task UpdateAsync(BaseColour baseColour);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
} 