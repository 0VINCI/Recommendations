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
            return Enumerable.Empty<(ProductEmbedding, float)>();

        // Pobierz wszystkie embeddingi oprócz źródłowego dla tego samego wariantu
        var allEmbeddings = await dbContext.ProductEmbeddings
            .Where(pe => pe.ProductId != productId && pe.Variant == variant)
            .ToListAsync(cancellationToken);

        // Oblicz podobieństwo i zwróć top X
        var similarProducts = new List<(ProductEmbedding, float)>();
        
        foreach (var pe in allEmbeddings)
        {
            var similarityScore = await GetSimilarityScoreAsync(sourceEmbedding.Embedding, pe.Embedding, cancellationToken);
            similarProducts.Add((pe, similarityScore));
        }

        return similarProducts
            .OrderByDescending(x => x.Item2)
            .Take(topCount)
            .ToList();
    }

    private async Task<float> GetSimilarityScoreAsync(Vector sourceVector, Vector targetVector, CancellationToken cancellationToken = default)
    {
        var connection = dbContext.Database.GetDbConnection() as NpgsqlConnection;
        if (connection == null)
            throw new InvalidOperationException("Database connection is not NpgsqlConnection");

        await using var cmd = new NpgsqlCommand("SELECT 1 - (@a <=> @b)", connection);
        cmd.Parameters.Add(new NpgsqlParameter("a", sourceVector));
        cmd.Parameters.Add(new NpgsqlParameter("b", targetVector));
        var result = await cmd.ExecuteScalarAsync(cancellationToken);
        var sim = result is not null ? (float)result : 0f;
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
