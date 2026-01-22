using Microsoft.EntityFrameworkCore;
using Npgsql;
using Recommendations.VisualBased.Core.Data;

namespace Recommendations.VisualBased.Core.Repositories;

internal sealed class VisualEmbeddingRepository(VisualBasedDbContext dbContext) : IVisualEmbeddingRepository
{
    public async Task<IEnumerable<(string ItemId, float SimilarityScore)>> GetSimilarItems(
        string itemId,
        int topCount,
        CancellationToken cancellationToken = default)
    {
        var sourceItem = await dbContext.ItemVisuals
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ItemId == itemId, cancellationToken);

        if (sourceItem == null)
            return [];

        var connection = dbContext.Database.GetDbConnection() as NpgsqlConnection;
        if (connection == null)
            throw new InvalidOperationException("Database connection is not NpgsqlConnection");

        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        const string sql = """
            SELECT i."ItemId",
                   1 - (i."Emb" <=> @sourceEmbedding) as similarity_score
            FROM "Visual"."item_embeddings_visual" i
            WHERE i."ItemId" != @itemId
            ORDER BY i."Emb" <=> @sourceEmbedding
            LIMIT @topCount
            """;

        await using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.Add(new NpgsqlParameter("sourceEmbedding", sourceItem.Emb));
        cmd.Parameters.Add(new NpgsqlParameter("itemId", itemId));
        cmd.Parameters.Add(new NpgsqlParameter("topCount", topCount));

        var results = new List<(string, float)>();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var resultItemId = reader.GetString(reader.GetOrdinal("ItemId"));
            var similarityScore = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("similarity_score")));
            results.Add((resultItemId, similarityScore));
        }

        return results;
    }

    public async Task<bool> ItemEmbeddingExists(string itemId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ItemVisuals
            .AsNoTracking()
            .AnyAsync(e => e.ItemId == itemId, cancellationToken);
    }

    public async Task<long> GetItemEmbeddingsCount(CancellationToken cancellationToken = default)
    {
        return await dbContext.ItemVisuals.CountAsync(cancellationToken);
    }
}

