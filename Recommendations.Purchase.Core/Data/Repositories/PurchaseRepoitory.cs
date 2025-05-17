using Recommendations.Purchase.Core.Data.EF;

namespace Recommendations.Purchase.Core.Data.Repositories;

internal sealed class PurchaseRepository(PurchaseDbContext dbContext) : IPurchaseRepository
{
}