using Recommendations.Dictionaries.Shared;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Dictionaries.Core.ModuleApi;
internal class DictionariesModuleApi(ICommandDispatcher commands,
        IQueryDispatcher queries)
    : IDictionariesModuleApi
{
}