namespace AoC.Common.Maps;

public static class MapSliceExtensions
{
    public static Map<T> Slice<T>(this Map<T> map, Rectangle rectangle) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(map, nameof(map));

        if (rectangle.Width + rectangle.Left > map.SizeX ||
            rectangle.Height + rectangle.Top > map.SizeY ||
            rectangle.Top < 0 || rectangle.Left < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rectangle), "Rectangle exceeds the map size");
        }

        var other = map.ToReadOnlySpan2D();
        var result = other[rectangle.Left..(rectangle.Left + rectangle.Width), rectangle.Top..(rectangle.Top + rectangle.Height)];

        return new Map<T>(result.ToArray());
    }
}
