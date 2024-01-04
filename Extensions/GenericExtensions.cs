using FluentSearchEngine.GenericEvaluators.Interfaces;

namespace FluentSearchEngine.Extensions;

public static class GenericExtensions
{
    public static IValue<T> IsNotNull<T>(this IValue<T> value)
    {
        value.Filter.Append(" IS NOT NULL");
        return value;
    }

    public static IValue<T> IsNull<T>(this IValue<T> value)
    {
        value.Filter.Append(" IS NULL");
        return value;
    }

    public static IValue<T> Exists<T>(this IValue<T> value)
    {
        value.Filter.Append(" EXISTS");
        return value;
    }
}
