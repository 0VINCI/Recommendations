using Microsoft.EntityFrameworkCore;
using Npgsql;
using Recommendations.ContentBased.Core.Repositories;
using Recommendations.ContentBased.Core.Types;
using Recommendations.ContentBased.Shared.Types;
using Pgvector;

namespace Recommendations.ContentBased.Core.Data.Repositories;

internal sealed class ProductEmbeddingRepository(ContentBasedDbContext dbContext) : IProductEmbeddingRepository
{
    public async Task<ProductEmbedding?> GetByProductIdAndVariant(Guid productId, VectorType variant, CancellationToken cancellationToken = default)
        => await dbContext.ProductEmbeddings
            .SingleOrDefaultAsync(pe => pe.ProductId == productId && pe.Variant == variant, cancellationToken);

    public async Task<IEnumerable<ProductEmbedding>> GetByProductId(Guid productId, CancellationToken cancellationToken = default)
        => await dbContext.ProductEmbeddings
            .Where(pe => pe.ProductId == productId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<ProductEmbedding>> GetAll(CancellationToken cancellationToken = default)
        => await dbContext.ProductEmbeddings
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<(ProductEmbedding ProductEmbedding, float SimilarityScore)>> GetSimilarProducts(
        Guid productId, 
        VectorType variant, 
        int topCount, 
        CancellationToken cancellationToken = default)
    {
        var sourceEmbedding = await GetByProductIdAndVariant(productId, variant, cancellationToken);
        if (sourceEmbedding == null)
            return [];

        // Use direct NpgsqlCommand for better control over data mapping
        var connection = dbContext.Database.GetDbConnection() as NpgsqlConnection;
        if (connection == null)
            throw new InvalidOperationException("Database connection is not NpgsqlConnection");

        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var sql = @"
            SELECT pe.""ProductId"", pe.""Variant"", pe.""Embedding"", pe.""CreatedAt"", pe.""UpdatedAt"",
                   1 - (pe.""Embedding"" <=> @sourceEmbedding) as similarity_score
            FROM ""Vectors"".""ProductEmbeddings"" pe
            WHERE pe.""ProductId"" != @productId AND pe.""Variant"" = @variant
            ORDER BY similarity_score DESC
            LIMIT @topCount";

        await using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.Add(new NpgsqlParameter("sourceEmbedding", sourceEmbedding.Embedding));
        cmd.Parameters.Add(new NpgsqlParameter("productId", productId));
        cmd.Parameters.Add(new NpgsqlParameter("variant", variant.ToString()));
        cmd.Parameters.Add(new NpgsqlParameter("topCount", topCount));

        var results = new List<(ProductEmbedding, float)>();
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        
        while (await reader.ReadAsync(cancellationToken))
        {
            var productIdFromDb = reader.GetGuid(reader.GetOrdinal("ProductId"));
            var variantFromDb = Enum.Parse<VectorType>(reader.GetString(reader.GetOrdinal("Variant")));
            var embedding = reader.GetFieldValue<Vector>(reader.GetOrdinal("Embedding"));
            var similarityScore = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("similarity_score")));
            
            var productEmbedding = ProductEmbedding.Create(productIdFromDb, variantFromDb, embedding);
            results.Add((productEmbedding, similarityScore));
        }

        return results;
    }

    private async Task<float> GetSimilarityScoreAsync(Vector sourceVector, Vector targetVector, CancellationToken cancellationToken = default)
    {
        var connection = dbContext.Database.GetDbConnection() as NpgsqlConnection;
        if (connection == null)
            throw new InvalidOperationException("Database connection is not NpgsqlConnection");

        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await using var cmd = new NpgsqlCommand("SELECT 1 - (@a <=> @b)", connection);
        cmd.Parameters.Add(new NpgsqlParameter("a", sourceVector));
        cmd.Parameters.Add(new NpgsqlParameter("b", targetVector));
        var result = await cmd.ExecuteScalarAsync(cancellationToken);
        var sim = result is not null ? Convert.ToSingle(result) : 0f;
        return sim;
    }

    public async Task Save(ProductEmbedding productEmbedding, CancellationToken cancellationToken = default)
    {
        var existing = await GetByProductIdAndVariant(productEmbedding.ProductId, productEmbedding.Variant, cancellationToken);
        if (existing != null)
        {
            existing.Update(productEmbedding.Embedding);
        }
        else
        {
            dbContext.ProductEmbeddings.Add(productEmbedding);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveRange(IEnumerable<ProductEmbedding> productEmbeddings, CancellationToken cancellationToken = default)
    {
        dbContext.ProductEmbeddings.AddRange(productEmbeddings);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid productId, VectorType variant, CancellationToken cancellationToken = default)
    {
        var productEmbedding = await GetByProductIdAndVariant(productId, variant, cancellationToken);
        if (productEmbedding != null)
        {
            dbContext.ProductEmbeddings.Remove(productEmbedding);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteByProductId(Guid productId, CancellationToken cancellationToken = default)
    {
        var productEmbeddings = await GetByProductId(productId, cancellationToken);
        if (productEmbeddings.Any())
        {
            dbContext.ProductEmbeddings.RemoveRange(productEmbeddings);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
