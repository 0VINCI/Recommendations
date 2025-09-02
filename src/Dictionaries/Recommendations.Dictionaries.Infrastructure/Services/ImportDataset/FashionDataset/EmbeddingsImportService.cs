using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using Recommendations.Dictionaries.Core.Services;
using Recommendations.Dictionaries.Infrastructure.DAL;

namespace Recommendations.Dictionaries.Infrastructure.Services.ImportDataset.FashionDataset;

public sealed class EmbeddingsImportService(
    DictionariesDbContext db,
    ILogger<EmbeddingsImportService> logger
) : IEmbeddingsImportService
{
    private static readonly (string FileName, string Variant)[] Variants =
    [
        ("product_vectors.csv", "Full"),
        ("product_vectors_no_brand.csv", "NoBrand"),
        ("product_vectors_no_brand_and_attributes.csv", "NoBrandAndAttributes"),
        ("product_vectors_only_description.csv", "OnlyDescription")
    ];

    private const string UpsertSql = """
        INSERT INTO "Vectors"."ProductEmbeddings" ("ProductId","Variant","Embedding","CreatedAt")
        SELECT p."Id", @variant, @embedding::vector, NOW()
        FROM "Dictionary"."Products" p
        WHERE p."ExternalId" = @externalId
        ON CONFLICT ("ProductId","Variant")
        DO UPDATE SET "Embedding" = EXCLUDED."Embedding",
                      "UpdatedAt" = NOW();
        """;

    public async Task ImportEmbeddingsFromCsvAsync(
        string dataDir,
        int batchSize = 1000,
        int reopenEvery = 5000,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dataDir))
            throw new ArgumentException("dataDir is empty", nameof(dataDir));
        if (!Directory.Exists(dataDir))
            throw new DirectoryNotFoundException($"Directory not found: {dataDir}");

        var connString = db.Database.GetDbConnection().ConnectionString;

        foreach (var (fileName, variant) in Variants)
        {
            var path = Path.Combine(dataDir, fileName);
            if (!File.Exists(path))
            {
                logger.LogWarning("CSV not found for variant {Variant}: {Path}", variant, path);
                continue;
            }

            logger.LogInformation("Starting import: {Variant} from {Path}", variant, path);

            var processed = 0;
            var upserted = 0;
            var parseErrors = 0;

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null,
                HeaderValidated = null,
                TrimOptions = TrimOptions.Trim
            };

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, csvConfig);

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync(ct);

            await using var cmd = new NpgsqlCommand(UpsertSql, conn);

            var pVariant = cmd.Parameters.Add("@variant", NpgsqlDbType.Text);
            var pEmbedding = cmd.Parameters.Add("@embedding", NpgsqlDbType.Text); // rzutowanie w SQL ::vector
            var pExternalId = cmd.Parameters.Add("@externalId", NpgsqlDbType.Text);

            pVariant.Value = variant;

            NpgsqlTransaction? tx = null;

            async Task ReopenAsync(CancellationToken token)
            {
                // zamknij poprzednią transakcję (jeśli jest)
                if (tx is not null)
                {
                    try { await tx.CommitAsync(token); } catch { /* ignore */ }
                    await tx.DisposeAsync();
                    tx = null;
                }

                if (conn.FullState != System.Data.ConnectionState.Closed)
                    await conn.CloseAsync();

                await conn.OpenAsync(token);

                tx = await conn.BeginTransactionAsync(token);
                cmd.Connection = conn;
                cmd.Transaction = tx;

                // SET LOCAL wymaga aktywnej transakcji
                await using (var cfg = new NpgsqlCommand("SET LOCAL synchronous_commit TO OFF; SET LOCAL statement_timeout TO '0';", conn, tx))
                    await cfg.ExecuteNonQueryAsync(token);

                await cmd.PrepareAsync(token);
            }

            // pierwsze „otwarcie” z konfiguracją sesji i prepare
            await ReopenAsync(ct);

            var batch = new List<(string ExternalId, string Embedding)>(capacity: batchSize);

            try
            {
                await foreach (dynamic rec in csv.GetRecordsAsync<dynamic>().WithCancellation(ct))
                {
                    processed++;

                    string externalId = rec.id?.ToString() ?? string.Empty;
                    string vectorRaw = rec.vector?.ToString() ?? string.Empty;

                    if (string.IsNullOrWhiteSpace(externalId) || string.IsNullOrWhiteSpace(vectorRaw))
                    {
                        parseErrors++;
                        continue;
                    }

                    var embedding = NormalizeVectorText(vectorRaw);
                    if (embedding is null)
                    {
                        parseErrors++;
                        continue;
                    }

                    batch.Add((externalId, embedding));

                    if (batch.Count >= batchSize)
                    {
                        upserted += await FlushAsync(cmd, batch, ct);
                        batch.Clear();
                    }

                    if (processed % reopenEvery == 0)
                    {
                        // zamknij transakcję/połączenie i otwórz na nowo
                        await ReopenAsync(ct);
                    }
                }

                if (batch.Count > 0)
                {
                    upserted += await FlushAsync(cmd, batch, ct);
                    batch.Clear();
                }

                // końcowy commit
                try { if (tx is not null) await tx.CommitAsync(ct); } catch { /* ignore */ }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Import failed for variant {Variant} at record {Processed}", variant, processed);
                // rollback na wszelki wypadek
                try { if (tx is not null) await tx.RollbackAsync(ct); } catch { /* ignore */ }
                throw;
            }

            logger.LogInformation(
                "Finished import: {Variant}. Processed: {Processed}, Upserted: {Upserted}, ParseErrors: {ParseErrors}",
                variant, processed, upserted, parseErrors);
        }
    }

    private static async Task<int> FlushAsync(NpgsqlCommand cmd, List<(string ExternalId, string Embedding)> batch, CancellationToken ct)
    {
        var count = 0;
        foreach (var (externalId, embedding) in batch)
        {
            cmd.Parameters["@externalId"].Value = externalId;
            cmd.Parameters["@embedding"].Value = embedding;
            await cmd.ExecuteNonQueryAsync(ct);
            count++;
        }
        return count;
    }

    private static string? NormalizeVectorText(string raw)
    {
        var s = raw.Trim();
        if (!s.StartsWith("[") || !s.EndsWith("]"))
        {
            if (s.Contains(',')) s = "[" + s + "]";
            else return null;
        }
        return s.Replace(", ", ",");
    }
}
