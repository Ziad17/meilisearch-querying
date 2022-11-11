using FluentSearchEngine.GenericEvaluators.Interfaces;
using System.Globalization;

namespace FluentSearchEngine.Extensions
{
    public static class NumericExtensions
    {
        public static IValue<T> IsEqual<T>(this INumbersEvaluator<T> value, decimal number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"= {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IValue<T>)value;
        }
        public static IValue<T> IsNotEqual<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"!= {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IValue<T>)value;
        }
        public static IValue<T> GreaterThan<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"> {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IValue<T>)value;
        }
        public static IValue<T> GreaterThanOrEquals<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($">= {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IValue<T>)value;
        }

        public static IValue<T> LowerThanOrEquals<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"<= {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IValue<T>)value;
        }
        public static IValue<T> LowerThan<T>(this INumbersEvaluator<T> value, int number)
        {
            value.Filter.Append(
                StringExtensions.AddWhiteSpaceBeforeToLower($"< {number.ToString(CultureInfo.InvariantCulture)}"));
            return (IValue<T>)value;
        }
    }
}
