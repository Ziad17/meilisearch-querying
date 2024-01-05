using FluentSearchEngine.GenericEvaluators.Interfaces;

namespace FluentSearchEngine.Extensions;

public static class GenericExtensions
{
    public static IValue<T> IsNotNull<T>(this IGenericEvaluator<T> value)
    {
        value.Filter.Append(" IS NOT NULL");
        return (IValue<T>)value;
    }

    public static IValue<T> IsNull<T>(this IGenericEvaluator<T> value)
    {
        value.Filter.Append(" IS NULL");
        return (IValue<T>)value;
    }

    public static IValue<T> Exists<T>(this IGenericEvaluator<T> value)
    {
        value.Filter.Append(" EXISTS");
        return (IValue<T>)value;
    }
}
