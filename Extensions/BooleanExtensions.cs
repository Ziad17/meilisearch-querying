using FluentSearchEngine.GenericEvaluators.Interfaces;

namespace FluentSearchEngine.Extensions
{
    public static class BooleanExtensions
    {
        public static IEvaluator<T> IsTrue<T>(this IBooleanEvaluator<T> value)
        {
            value.Filter.Append(StringExtensions.AddWhiteSpaceBeforeToLower("= true"));
            return (IEvaluator<T>)value;
        }

        public static IEvaluator<T> IsFalse<T>(this IBooleanEvaluator<T> value)
        {
            value.Filter.Append(StringExtensions.AddWhiteSpaceBeforeToLower("= false"));
            return (IEvaluator<T>)value;
        }
    }
}
