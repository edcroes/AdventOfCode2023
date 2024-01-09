namespace AoC.Common.Maps;

public static class MapForEachExtensions
{
    public static Map<T> ForEachInTriangle<T>(this Map<T> map, Point first, Point second, Point third, Action<Point, T> action) where T : notnull
    {
        if (!map.Contains(first))
            throw new ArgumentOutOfRangeException(nameof(first), "Point should be in the map");

        if (!map.Contains(second))
            throw new ArgumentOutOfRangeException(nameof(second), "Point should be in the map");

        if (!map.Contains(third))
            throw new ArgumentOutOfRangeException(nameof(third), "Point should be in the map");

        Line line1 = new(first.ToLongPoint(), second.ToLongPoint());
        Line line2 = new(second.ToLongPoint(), third.ToLongPoint());
        Line line3 = new(first.ToLongPoint(), third.ToLongPoint());

        var trianglePoints = line1.GetLinePoints()
            .Union(line2.GetLinePoints())
            .Union(line3.GetLinePoints())
            .Select(p => p.ToPoint())
            .ToArray();

        var minY = trianglePoints.Min(p => p.Y);
        var maxY = trianglePoints.Max(p => p.Y);

        for (var y = minY; y <= maxY; y++)
        {
            var rowPoints = trianglePoints.Where(p => p.Y == y).ToArray();
            var minX = rowPoints.Min(p => p.X);
            var maxX = rowPoints.Max(p => p.X);

            for (var x = minX; x <= maxX; x++)
            {
                Console.WriteLine($"{x}, {y}");
                action(new(x, y), map.GetValue(x, y));
            }
        }

        return map;
    }

    public static Map<T> ForEachInRectangle<T>(this Map<T> map, Rectangle rectangle, Action<Point, T> action) where T : notnull
    {
        if (rectangle.Width + rectangle.Left > map.SizeX ||
            rectangle.Height + rectangle.Top > map.SizeY ||
            rectangle.Top < 0 || rectangle.Left < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rectangle), "Rectangle exceeds the map size");
        }

        for (var y = rectangle.Top; y < rectangle.Top + rectangle.Height; y++)
        {
            for (var x = rectangle.Left; x < rectangle.Left + rectangle.Width; x++)
            {
                action(new(x, y), map.GetValue(x, y));
            }
        }

        return map;
    }

    public static Map<T> ForEach<T>(this Map<T> map, ForEachOrder order, Action<Point, T> action) where T : notnull
    {
        var xPositive = (0, map.SizeX - 1);
        var xNegative = (map.SizeX - 1, 0);
        var yPositive = (0, map.SizeY - 1);
        var yNegative = (map.SizeY - 1, 0);

        return order switch
        {
            ForEachOrder.LeftRightTopBottom => map.ForEach(action),
            ForEachOrder.LeftRightBottomTop => map.ForEach(xPositive, yNegative, action),
            ForEachOrder.RightLeftTopBottom => map.ForEach(xNegative, yPositive, action),
            ForEachOrder.RightLeftBottomTop => map.ForEach(xNegative, yNegative, action),
            ForEachOrder.TopBottomLeftRight => map.ForEach(yPositive, xPositive, action, false),
            ForEachOrder.TopBottomRightLeft => map.ForEach(yPositive, xNegative, action, false),
            ForEachOrder.BottomTopLeftRight => map.ForEach(yNegative, xPositive, action, false),
            ForEachOrder.BottomTopRightLeft => map.ForEach(yNegative, xNegative, action, false),
            _ => throw new NotSupportedException($"{order} is not a supported foreach loop")
        };
    }

    public static Map<T> ForEach<T>(this Map<T> map, Action<Point, T> action) where T : notnull =>
        map.ForEach((0, map.SizeX - 1), (0, map.SizeY - 1), action);

    private static Map<T> ForEach<T>(this Map<T> map, ValueTuple<int, int> x, ValueTuple<int, int> y, Action<Point, T> action, bool xFirst = true) where T : notnull
    {
        var (yFrom, yTo) = y;
        var yPositive = yFrom < yTo;

        for (; yPositive ? yFrom <= yTo : yFrom >= yTo; yFrom += yPositive ? 1 : -1)
        {
            var (xFrom, xTo) = x;
            var xPositive = xFrom < xTo;

            for (; xPositive ? xFrom <= xTo : xFrom >= xTo; xFrom += xPositive ? 1 : -1)
            {
                if (xFirst)
                    action(new Point(xFrom, yFrom), map.GetValue(xFrom, yFrom));
                else
                    action(new Point(yFrom, xFrom), map.GetValue(yFrom, xFrom));
            }
        }

        return map;
    }
}
