using FluentSearchEngine.GenericEvaluators.Interfaces;

namespace FluentSearchEngine.Extensions
{
    public static class CollectionExpressions
    {
        public static IValue<T> Contains<T, TData>(this ICollectionEvaluator<T> value, TData itemToSearch)
        {
            value.Filter.Append(StringExtensions.AddWhiteSpaceBeforeToLower($"= '{itemToSearch}'"));
            return (IValue<T>)value;
        }
    }
}
