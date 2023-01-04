using FluentSearchEngine.Paging;
using Meilisearch;
using System.Linq.Expressions;

namespace FluentSearchEngine.GenericEvaluators.Interfaces
{
    public interface IValue<T> : IFilter
    {
        IStringsEvaluator<T> Value(Expression<Func<T, string>> action);

        ICollectionEvaluator<T> Value<TData>(Expression<Func<T, ICollection<TData>>> action);

        IBooleanEvaluator<T> Value(Expression<Func<T, bool>> action);

        INumbersEvaluator<T> Value(Expression<Func<T, int>> action);

        INumbersEvaluator<T> Value(Expression<Func<T, double>> action);

        INumbersEvaluator<T> Value(Expression<Func<T, decimal>> action);

        SearchQuery Evaluate(PageCriteria pageCriteria);

        SearchQuery Evaluate(int pageNumber = 1, int pageSize = 10);
    }
}
