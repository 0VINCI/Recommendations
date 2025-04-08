using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Shared.Infrastructure.Queries; 

internal sealed class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync), 
            new[] { query.GetType(), typeof(CancellationToken) });

        return await (Task<TResult>) method.Invoke(handler, new object[] { query, cancellationToken });

    }
}
