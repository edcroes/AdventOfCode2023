using System.Numerics;

namespace AoC.Common.Maps;

public readonly record struct Point3D<T>(T X, T Y, T Z) where T : INumber<T>
{
    public static Point3D<T> Empty => new(T.Zero, T.Zero, T.Zero);

    public override string ToString() => $"({X}, {Y}, {Z})";

    public static Point3D<T> operator +(Point3D<T> left, Point3D<T> right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    public static Point3D<T> operator -(Point3D<T> left, Point3D<T> right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
}