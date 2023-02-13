using FluentSearchEngine.Paging;
using Meilisearch;
using System.Linq.Expressions;

namespace FluentSearchEngine.GenericEvaluators.Interfaces
{
    public interface IValue<T> : IFilter
    {
        public SearchQuery Evaluate(PageCriteria pagingOptions);

        public IBooleanEvaluator<T> Value(Expression<Func<T, bool>> action);

        public INumbersEvaluator<T> Value(Expression<Func<T, decimal>> action);

        public INumbersEvaluator<T> Value(Expression<Func<T, int>> action);

        public INumbersEvaluator<T> Value(Expression<Func<T, double>> action);

        public ICollectionEvaluator<T> Value<TData>(Expression<Func<T, ICollection<TData>>> action);

        public IDateTimeEvaluator<T> Value(Expression<Func<T, DateTime>> action);

        public IStringsEvaluator<T> Value(Expression<Func<T, string>> action);

        public SearchQuery Evaluate(int pageNumber = 1, int pageSize = 10);
    }
}
