using System.Text;

namespace AoC.Common.Geometry;

public static class LineLogExtensions
{
    public static void DumpLinesToConsole(this IEnumerable<Line> lines) =>
        Console.WriteLine(lines.DumpLinesToString());

    public static async Task DumpLinesToFile(this IEnumerable<Line> lines, string filePath)
    {
        var linesDump = lines.DumpLinesToString();
        await File.WriteAllTextAsync(filePath, linesDump);
    }

    public static string DumpLinesToString(this IEnumerable<Line> lines)
    {
        var allPoints = lines.SelectMany(l => l.GetLinePoints()).ToList();

        StringBuilder builder = new();
        var minX = allPoints.Min(p => p.X);
        var maxX = allPoints.Max(p => p.X);
        var minY = allPoints.Min(p => p.Y);
        var maxY = allPoints.Max(p => p.Y);

        for (var y = minY; y <= 0; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                Point<long> point = new(x, y);
                var draw = allPoints.Contains(point)
                    ? '#'
                    : '.';
                builder.Append(draw);
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
