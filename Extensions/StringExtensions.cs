using FluentSearchEngine.GenericEvaluators.Interfaces;

namespace FluentSearchEngine.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToLowerCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
                return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

            return str;
        }

        public static IValue<T> ExactSame<T>(this IStringsEvaluator<T> value, string text)
        {
            value.Filter.Append($" = '{text}'");
            return (IValue<T>)value;
        }
        
        public static IValue<T> NotExactSame<T>(this IStringsEvaluator<T> value, string text)
        {
            value.Filter.Append($" != '{text}'");
            return (IValue<T>)value;
        }

        public static IValue<T> IsEmpty<T>(this IStringsEvaluator<T> value)
        {
            value.Filter.Append(" IS EMPTY");
            return (IValue<T>)value;
        }

        public static IValue<T> IsNotEmpty<T>(this IStringsEvaluator<T> value)
        {
            value.Filter.Append(" IS NOT EMPTY");
            return (IValue<T>)value;
        }
    }
}
