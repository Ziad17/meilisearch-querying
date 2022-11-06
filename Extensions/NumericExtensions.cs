using FluentSearchEngine.GenericEvaluators.Interfaces;
using System.Globalization;

namespace FluentSearchEngine.Extensions
{
    public static class NumericExtensions
    {
        public static IEvaluator<T> IsEqual<T>(this INumbersEvaluator<T> value, decimal number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"= {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IEvaluator<T>)value;
        }
        public static IEvaluator<T> IsNotEqual<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"!= {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IEvaluator<T>)value;
        }
        public static IEvaluator<T> GreaterThan<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"> {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IEvaluator<T>)value;
        }
        public static IEvaluator<T> GreaterThanOrEquals<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($">= {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IEvaluator<T>)value;
        }

        public static IEvaluator<T> LowerThanOrEquals<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"<= {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IEvaluator<T>)value;
        }
        public static IEvaluator<T> LowerThan<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"< {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IEvaluator<T>)value;
        }
    }
}
