namespace AoC.Common.Maps;
public static class MapImagingExtensions
{
    private static readonly Dictionary<(Point previous, Point next), (Point[] inner, Point[] outer)> _innerOuterTranslation = new()
    {
        // -
        { (new(-1, 0), new(1, 0)), ([new(0, 1)], [new(0, -1)]) },
        { (new(1, 0), new(-1, 0)), ([new(0, -1)], [new(0, 1)]) },
        // |
        { (new(0, -1), new(0, 1)), ([new(-1, 0)], [new(1, 0)]) },
        { (new(0, 1), new(0, -1)), ([new(1, 0)], [new(-1, 0)]) },
        // F
        { (new(0, 1), new(1, 0)), ([], [new(-1, 0), new(-1, -1), new(0, -1)]) },
        { (new(1, 0), new(0, 1)), ([new(-1, 0), new(-1, -1), new(0, -1)], []) },
        // L
        { (new(1, 0), new(0, -1)), ([], [new(-1, 0), new(-1, 1), new(0, 1)]) },
        { (new(0, -1), new(1, 0)), ([new(-1, 0), new(-1, 1), new(0, 1)], []) },
        // 7
        { (new(-1, 0), new(0, 1)), ([], [new(0, -1), new(1, -1), new(1, 0)]) },
        { (new(0, 1), new(-1, 0)), ([new(0, -1), new(1, -1), new(1, 0)], []) },
        // J
        { (new(0, -1), new(-1, 0)), ([], [new(1, 0), new(1, 1), new(0, 1)]) },
        { (new(-1, 0), new(0, -1)), ([new(0, -1), new(1, -1), new(1, 0)], []) }
    };

    public static Map<T> DrawShape<T>(this Map<T> map, IEnumerable<Point> border, T borderValue) =>
        map.DrawShape(border, _ => borderValue);

    public static Map<T> DrawShape<T>(this Map<T> map, IEnumerable<Point> border, Func<Point, T> getBorderValue)
    {
        ArgumentNullException.ThrowIfNull(map);
        ArgumentNullException.ThrowIfNull(border);
        ArgumentNullException.ThrowIfNull(getBorderValue);

        var borderPoints = border.ToArray();
        if (borderPoints.Length < 4 || !IsShapeValid(borderPoints))
            throw new ArgumentNullException(nameof(border));

        foreach (var point in border)
        {
            map.SetValue(point, getBorderValue(point));
        }

        return map;
    }

    public static Map<T> FillShape<T>(this Map<T> map, IEnumerable<Point> border, T borderValue, T fillValue) =>
        map.FillShape(border, _ => borderValue, _ => fillValue);

    public static Map<T> FillShape<T>(this Map<T> map, IEnumerable<Point> border, Func<Point, T> getBorderValue, T fillValue) =>
        map.FillShape(border, getBorderValue, _ => fillValue);

    public static Map<T> FillShape<T>(this Map<T> map, IEnumerable<Point> border, Func<Point, T> getBorderValue, Func<Point, T> getFillValue)
    {
        ArgumentNullException.ThrowIfNull(map);
        ArgumentNullException.ThrowIfNull(border);
        ArgumentNullException.ThrowIfNull(getBorderValue);
        ArgumentNullException.ThrowIfNull(getFillValue);

        var borderPoints = border.ToArray();
        if (borderPoints.Length < 4 || !IsShapeValid(borderPoints))
            throw new ArgumentNullException(nameof(border));

        List<Point> inner = [];
        List<Point> outer = [];

        for (var i = 0; i < borderPoints.Length; i++)
        {
            var prev = i == 0 ? borderPoints[^1] : borderPoints[i - 1];
            var next = i == borderPoints.Length - 1 ? borderPoints[0] : borderPoints[i + 1];
            var current = borderPoints[i];

            var (inners, outers) = _innerOuterTranslation[(prev.Subtract(current), next.Subtract(current))];
            inner.AddRange(inners.Select(i => i.Add(current)).Where(p => !borderPoints.Contains(p)));
            outer.AddRange(outers.Select(o => o.Add(current)).Where(p => !borderPoints.Contains(p)));

            map.SetValue(current, getBorderValue(current));
        }

        var realInner = inner.Min(p => p.X) > outer.Min(p => p.X) ? inner : outer;
        foreach (var point in realInner)
        {
            if (getFillValue(point).Equals(map.GetValue(point)))
                continue;

            map.SetValue(point, getFillValue(point));
            map.FloodFill(point, p => !borderPoints.Contains(p));
        }

        return map;
    }

    private static void FloodFill<T>(this Map<T> map, Point from, Func<Point, bool> shouldFill)
    {
        var value = map.GetValue(from);
        var neighbors = map.GetStraightNeighbors(from);

        foreach (var neighbor in neighbors.Where(n => shouldFill(n)))
        {
            if (!map.GetValue(neighbor).Equals(value))
            {
                map.SetValue(neighbor, value);
                map.FloodFill(neighbor, shouldFill);
            }
        }
    }

    private static bool IsShapeValid(Point[] border)
    {
        Point[] valid = [new(-1, 0), new(1, 0), new(0, -1), new(0, 1)];
        var previous = border[^1];
        foreach (var point in border)
        {
            var diff = previous.Subtract(point);
            if (!valid.Contains(diff))
                return false;
            previous = point;
        }

        return true;
    }
}
