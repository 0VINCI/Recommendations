namespace Recommendations.Dictionaries.Core.Services;

public interface IDataImportService
{
    Task ImportCsvDataAsync(string stylesCsvPath, string imagesCsvPath);

    Task ImportJsonDataAsync(string jsonDirectoryPath);
}