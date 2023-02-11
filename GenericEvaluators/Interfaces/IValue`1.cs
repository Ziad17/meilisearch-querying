using FluentSearchEngine.Paging;
using Meilisearch;

namespace FluentSearchEngine.GenericEvaluators.Interfaces
{
    public interface IValue<T> : IFilter
    {
        public SearchQuery Evaluate(PageCriteria pagingOptions);

        public SearchQuery Evaluate(int pageNumber = 1, int pageSize = 10);
    }
}
