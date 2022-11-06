using FluentSearchEngine.Paging;
using Meilisearch;

namespace FluentSearchEngine.GenericEvaluators.Interfaces
{
    public interface IEvaluator<T> : IFilter
    {
        public SearchQuery Evaluate(PageCriteria? pageCriteria = default);
        public IValue<T> And();
        public IValue<T> Or();

    }
}
