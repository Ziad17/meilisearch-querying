using FluentSearchEngine.GenericEvaluators.Interfaces;
using System.Globalization;

namespace FluentSearchEngine.Extensions
{
    public static class NumericExtensions
    {
        public static IValue<T> ApplyOperand<T>(this INumbersEvaluator<T> value, string operand, string number)
        {
            value.Filter.AppendWithWhiteSpace($"{operand} {number}");
            return (IValue<T>)value;
        }

        public static IValue<T> EqualTo<T>(this INumbersEvaluator<T> value, int number)
        {
            return value.ApplyOperand("=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> NotEqualTo<T>(this INumbersEvaluator<T> value, int number)
        {
            return value.ApplyOperand("!=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> GreaterThan<T>(this INumbersEvaluator<T> value, int number)
        {
            return value.ApplyOperand(">", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> GreaterThanOrEquals<T>(this INumbersEvaluator<T> value, int number)
        {
            return value.ApplyOperand(">=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> LowerThanOrEquals<T>(this INumbersEvaluator<T> value, int number)
        {
            return value.ApplyOperand("<=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> LowerThan<T>(this INumbersEvaluator<T> value, int number)
        {
            return value.ApplyOperand("<", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> Equals<T>(this INumbersEvaluator<T> value, double number)
        {
            return value.ApplyOperand("=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> NotEqualTo<T>(this INumbersEvaluator<T> value, double number)
        {
            return value.ApplyOperand("!=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> GreaterThan<T>(this INumbersEvaluator<T> value, double number)
        {
            return value.ApplyOperand(">", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> GreaterThanOrEquals<T>(this INumbersEvaluator<T> value, double number)
        {
            return value.ApplyOperand(">=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> LowerThanOrEquals<T>(this INumbersEvaluator<T> value, double number)
        {
            return value.ApplyOperand("<=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> LowerThan<T>(this INumbersEvaluator<T> value, double number)
        {
            return value.ApplyOperand("<", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> Equals<T>(this INumbersEvaluator<T> value, decimal number)
        {
            return value.ApplyOperand("=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> NotEqualTo<T>(this INumbersEvaluator<T> value, decimal number)
        {
            return value.ApplyOperand("!=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> GreaterThan<T>(this INumbersEvaluator<T> value, decimal number)
        {
            return value.ApplyOperand(">", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> GreaterThanOrEquals<T>(this INumbersEvaluator<T> value, decimal number)
        {
            return value.ApplyOperand(">=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> LowerThanOrEquals<T>(this INumbersEvaluator<T> value, decimal number)
        {
            return value.ApplyOperand("<=", number.ToString(CultureInfo.InvariantCulture));
        }

        public static IValue<T> LowerThan<T>(this INumbersEvaluator<T> value, decimal number)
        {
            return value.ApplyOperand("<", number.ToString(CultureInfo.InvariantCulture));
        }
    }
}
