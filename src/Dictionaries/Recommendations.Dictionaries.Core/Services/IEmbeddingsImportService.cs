namespace Recommendations.Dictionaries.Core.Services;

public interface IEmbeddingsImportService
{
    /// <summary>
    /// Importuje cztery warianty embeddingów z plików CSV znajdujących się w katalogu dataDir.
    /// Oczekiwane nazwy plików:
    ///   - product_vectors.csv
    ///   - product_vectors_no_brand.csv
    ///   - product_vectors_no_brand_and_attributes.csv
    ///   - product_vectors_only_description.csv
    /// </summary>
    Task ImportEmbeddingsFromCsvAsync(string dataDir, int batchSize = 1000, int reopenEvery = 5000, CancellationToken ct = default);
}