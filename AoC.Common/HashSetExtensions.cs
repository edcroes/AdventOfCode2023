namespace AoC.Common;
public static class HashSetExtensions
{
    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            set.Add(value);
        }
    }
}
