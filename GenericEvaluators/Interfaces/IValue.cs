using FluentSearchEngine.Paging;
using Meilisearch;
using System.Linq.Expressions;

namespace FluentSearchEngine.GenericEvaluators.Interfaces
{
    public interface IValue<T> : IFilter
    {
        public IStringsEvaluator<T> And(Expression<Func<T, string>> action);
        public IStringsEvaluator<T> Or(Expression<Func<T, string>> action);


        public INumbersEvaluator<T> And(Expression<Func<T, int>> action);
        public INumbersEvaluator<T> Or(Expression<Func<T, int>> action);


        public IBooleanEvaluator<T> And(Expression<Func<T, bool>> action);
        public IBooleanEvaluator<T> Or(Expression<Func<T, bool>> action);


        public SearchQuery Evaluate(PageCriteria pageCriteria);
        public SearchQuery Evaluate(int pageNumber = 1, int pageSize = 10);

    }
}
