using FluentSearchEngine.GenericEvaluators.Interfaces;

namespace FluentSearchEngine.Extensions
{
    public static class BooleanExtensions
    {
        public static IValue<T> IsTrue<T>(this IBooleanEvaluator<T> value)
        {
            value.Filter.AppendWithWhiteSpace("= true");
            return (IValue<T>)value;
        }

        public static IValue<T> IsFalse<T>(this IBooleanEvaluator<T> value)
        {
            value.Filter.AppendWithWhiteSpace("= false");
            return (IValue<T>)value;
        }
    }
}
