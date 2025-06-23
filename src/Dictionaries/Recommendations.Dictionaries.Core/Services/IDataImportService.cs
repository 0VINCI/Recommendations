namespace Recommendations.Dictionaries.Core.Services;

public interface IDataImportService
{
    Task ImportFashionDatasetAsync(string stylesCsvPath, string imagesCsvPath);
    Task ImportJsonDataAsync(string jsonDirectoryPath);
} 