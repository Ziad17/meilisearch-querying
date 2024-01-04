using FluentSearchEngine.GenericEvaluators.Interfaces;

namespace FluentSearchEngine.Extensions
{
    public static class DateTimeExtensions
    {
        public static IValue<T> After<T>(this IDateTimeEvaluator<T> value, DateTime dateTime)
        {
            value.Filter.Append($" > {dateTime.ToUnixEpoch()}");
            return (IValue<T>)value;
        }

        public static IValue<T> AfterOrEqual<T>(this IDateTimeEvaluator<T> value, DateTime dateTime)
        {
            value.Filter.Append($" >= {dateTime}");
            return (IValue<T>)value;
        }

        public static double ToUnixEpoch(this DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var diff = date.ToUniversalTime() - origin;
            
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
