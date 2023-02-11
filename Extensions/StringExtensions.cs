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

        public static string AddWhiteSpaceBeforeToLower(object value)
        {
            var originalText = " " + value.ToString()!.FirstCharToLowerCase();
            return originalText;
        }

        public static IValue<T> Like<T>(this IStringsEvaluator<T> value, string text)
        {
            value.Filter.Append(AddWhiteSpaceBeforeToLower($"LIKE '{text}'"));
            return (IValue<T>)value;
        }

        public static IValue<T> ExactSame<T>(this IStringsEvaluator<T> value, string text)
        {
            value.Filter.Append(AddWhiteSpaceBeforeToLower($"= '{text}'"));
            return (IValue<T>)value;
        }
    }
}
