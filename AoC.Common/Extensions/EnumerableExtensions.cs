namespace AoC.Common.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> IntersectMany<T>(this IEnumerable<IEnumerable<T>> sources) =>
        sources
            .Skip(1)
            .Aggregate(sources.First(), (result, next) => result.Intersect(next));

    public static int GetAoCHashCode<T>(this IEnumerable<T> source) =>
        source.Aggregate(0, (result, next) => next is null ? result : HashCode.Combine(result, next.GetHashCode()));

    public static string ToReadableString<T>(this IEnumerable<T> set) =>
        string.Join(", ", set);
}
