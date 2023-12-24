using System.Numerics;

namespace AoC.Common.Files;

public static class LineParser
{
    public static int[] ToIntArray(this string line) =>
        line
            .ToCharArray()
            .Select(c => int.Parse(c.ToString()))
            .ToArray();

    public static int[] ToIntArray(this string line, string separator) =>
        line
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(c => int.Parse(c.ToString()))
            .ToArray();

    public static long[] ToLongArray(this string line, string separator) =>
        line
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(c => long.Parse(c.ToString()))
            .ToArray();

    public static bool[] ToBoolArray(this string line, char trueValue) =>
        line
            .ToCharArray()
            .Select(c => c == trueValue)
            .ToArray();

    public static T[] ToArray<T>(this string line, string separator) where T : INumber<T> =>
        line
            .Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(c => T.Parse(c.ToString(), null))
            .ToArray();

    public static Point3D<T> ToPoint3D<T>(this string line, string separator) where T : INumber<T>
    {
        var (x, y, z) = line.ToArray<T>(separator);
        return new(x!, y!, z!);
    }

    public static string[] ToStringArray(this string line, string separator) =>
        line.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
