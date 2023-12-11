using System.Numerics;
using System.Text;

namespace AoC.Common.Maps;

public static class PointMapLogExtensions
{
    public static void DumpMapToConsole<TPointType, TValue>(this PointMap<TPointType, TValue> map, Func<TValue, char> mapToChar) where TPointType : INumber<TPointType> =>
        Console.WriteLine(map.DumpMapToString(mapToChar));

    public static async Task DumpMapToFile<TPointType, TValue>(this PointMap<TPointType, TValue> map, string filePath, Func<TValue, char> mapToChar) where TPointType : INumber<TPointType>
    {
        var mapDump = map.DumpMapToString(mapToChar);
        await File.WriteAllTextAsync(filePath, mapDump);
    }

    public static string DumpMapToString<TPointType, TValue>(this PointMap<TPointType, TValue> map, Func<TValue, char> mapToChar) where TPointType : INumber<TPointType>
    {
        var bounds = map.GetBoundingRectangle();
        StringBuilder builder = new();

        for (var y = bounds.Y; y <= bounds.Height; y++)
        {
            for (var x = bounds.X; x <= bounds.Width; x++)
            {
                builder.Append(mapToChar(map.GetValueOrDefault(x, y)));
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
