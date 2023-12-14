namespace AoC.Common.Maps;
public static class MapForEachExtensions
{
    public static Map<T> ForEach<T>(this Map<T> map, ForEachOrder order, Action<Point, T> action)
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

    public static Map<T> ForEach<T>(this Map<T> map, Action<Point, T> action) =>
        map.ForEach((0, map.SizeX - 1), (0, map.SizeY - 1), action);

    private static Map<T> ForEach<T>(this Map<T> map, ValueTuple<int, int> x, ValueTuple<int, int> y, Action<Point, T> action, bool xFirst = true)
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
