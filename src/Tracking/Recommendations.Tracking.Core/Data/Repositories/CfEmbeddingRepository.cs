using Microsoft.EntityFrameworkCore;
using Npgsql;
using Recommendations.Tracking.Core.Data.Signals;
using Recommendations.Tracking.Core.Repositories;

namespace Recommendations.Tracking.Core.Data.Repositories;

internal sealed class CfEmbeddingRepository(SignalsDbContext dbContext) : ICfEmbeddingRepository
{
    public async Task<IEnumerable<(string ItemId, float SimilarityScore)>> GetSimilarItems(
        string itemId,
        int topCount,
        CancellationToken cancellationToken = default)
    {
        var sourceItem = await dbContext.ItemEmbeddingsCf
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
            FROM "Tracking"."item_embeddings_cf" i
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

    public async Task<IEnumerable<(string ItemId, float SimilarityScore)>> GetRecommendationsForUser(
        string userKey,
        int topCount,
        CancellationToken cancellationToken = default)
    {
        var userEmbedding = await dbContext.UserEmbeddingsCf
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserKey == userKey, cancellationToken);

        if (userEmbedding == null)
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
                   1 - (i."Emb" <=> @userEmbedding) as similarity_score
            FROM "Tracking"."item_embeddings_cf" i
            ORDER BY i."Emb" <=> @userEmbedding
            LIMIT @topCount
            """;

        await using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.Add(new NpgsqlParameter("userEmbedding", userEmbedding.Emb));
        cmd.Parameters.Add(new NpgsqlParameter("topCount", topCount));

        var results = new List<(string, float)>();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var itemId = reader.GetString(reader.GetOrdinal("ItemId"));
            var similarityScore = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("similarity_score")));
            results.Add((itemId, similarityScore));
        }

        return results;
    }

    public async Task<bool> UserEmbeddingExists(string userKey, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserEmbeddingsCf
            .AsNoTracking()
            .AnyAsync(e => e.UserKey == userKey, cancellationToken);
    }

    public async Task<bool> ItemEmbeddingExists(string itemId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ItemEmbeddingsCf
            .AsNoTracking()
            .AnyAsync(e => e.ItemId == itemId, cancellationToken);
    }

    public async Task<int> GetUserEmbeddingsCount(CancellationToken cancellationToken = default)
    {
        return await dbContext.UserEmbeddingsCf.CountAsync(cancellationToken);
    }

    public async Task<int> GetItemEmbeddingsCount(CancellationToken cancellationToken = default)
    {
        return await dbContext.ItemEmbeddingsCf.CountAsync(cancellationToken);
    }
}

