using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Dictionaries.Shared.DTO;

public record GetProductFullById(Guid Id) : IQuery<ProductFullDto?>; 