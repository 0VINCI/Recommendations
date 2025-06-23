using AutoMapper;
using Recommendations.Dictionaries.Core.Repositories;
using Recommendations.Dictionaries.Shared.DTO;
using Recommendations.Dictionaries.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Application.Queries.Handlers;

internal sealed class SearchProductsHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<SearchProducts, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(SearchProducts query, CancellationToken cancellationToken = default)
    {
        var products = await productRepository.SearchAsync(query.SearchTerm);
        return mapper.Map<IReadOnlyCollection<ProductDto>>(products);
    }
} 