using System.Linq.Expressions;

namespace FluentSearchEngine.GenericEvaluators.Interfaces
{
    public interface IValue<T> : IFilter
    {
        public INumbersEvaluator<T> Value(Expression<Func<T, int>> action);

        public IStringsEvaluator<T> Value(Expression<Func<T, string>> action);

        public IBooleanEvaluator<T> Value(Expression<Func<T, bool>> action);

    }
}
