using Recommendations.Dictionaries.Core.Types;

namespace Recommendations.Dictionaries.Core.Repositories;

public interface IArticleTypeRepository
{
    Task<ArticleType?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<ArticleType>> GetAllAsync();
    Task<ArticleType?> GetByNameAsync(string name);
    Task<IReadOnlyCollection<ArticleType>> GetBySubCategoryAsync(Guid subCategoryId);
    Task AddAsync(ArticleType articleType);
    Task UpdateAsync(ArticleType articleType);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
} 