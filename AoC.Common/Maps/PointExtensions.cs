namespace AoC.Common.Maps;

public static class PointExtensions
{
    public static Point Left => new(-1, 0);
    public static Point Right => new(1, 0);
    public static Point Up => new(0, -1);
    public static Point Down => new(0, 1);

    public static int GetManhattenDistance(this Point point, Point other) =>
        Math.Abs(point.X - other.X) + Math.Abs(point.Y - other.Y);

    public static Point Add(this Point left, Point right) =>
        new(left.X + right.X, left.Y + right.Y);

    public static Point Subtract(this Point left, Point right) =>
        new(left.X - right.X, left.Y - right.Y);

    public static bool IsTouching(this Point point, Point other) =>
        Math.Abs(point.X - other.X) <= 1 && Math.Abs(point.Y - other.Y) <= 1;

    public static Point<long> ToLongPoint(this Point point) =>
        new(point.X, point.Y);

    public static Point ToPoint(this Point<long> point)
    {
        if (point.X > int.MaxValue || point.X < int.MinValue ||
            point.Y > int.MaxValue || point.Y < int.MinValue)
        {
            throw new ArgumentOutOfRangeException(nameof(point));
        }

        return new((int)point.X, (int)point.Y);
    }
}
