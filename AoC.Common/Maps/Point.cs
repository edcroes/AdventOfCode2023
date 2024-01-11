using System.Numerics;

namespace AoC.Common.Maps;

public record struct Point<T>(T X, T Y) where T : INumber<T>
{
    public static Point<T> Left => new(-T.One, T.Zero);
    public static Point<T> Right => new(T.One, T.Zero);
    public static Point<T> Up => new(T.Zero, -T.One);
    public static Point<T> Down => new(T.Zero, T.One);

    public readonly T GetManhattenDistance(Point<T> other) =>
        T.Abs(X - other.X) + T.Abs(Y - other.Y);

    public readonly Point<T> Add(Point<T> other) =>
        new(X + other.X, Y + other.Y);

    public readonly Point<T> Subtract(Point<T> other) =>
        new(X - other.X, Y - other.Y);

    public readonly bool IsTouching(Point<T> other) =>
        T.Abs(X - other.X) <= T.One && T.Abs(Y - other.Y) <= T.One;
}
