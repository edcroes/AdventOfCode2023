using System.Numerics;

namespace AoC.Common.Maps;

public static class Point3DExtensions
{
    public static Point3D<T> MirrorX<T>(this Point3D<T> point) where T : INumber<T> =>
        new(point.X * -T.One, point.Y, point.Z);

    public static Point3D<T> MirrorY<T>(this Point3D<T> point) where T : INumber<T> =>
        new(point.X, point.Y * -T.One, point.Z);

    public static Point3D<T> MirrorZ<T>(this Point3D<T> point) where T : INumber<T> =>
        new(point.X, point.Y, point.Z * -T.One);

    public static Point3D<T> RotateX<T>(this Point3D<T> point) where T : INumber<T> =>
        new(point.Y, point.X * -T.One, point.Z);

    public static Point3D<T> RotateY<T>(this Point3D<T> point) where T : INumber<T> =>
        new(point.Z * -T.One, point.Y, point.X);

    public static Point3D<T> RotateZ<T>(this Point3D<T> point) where T : INumber<T> =>
        new(point.X, point.Z * -T.One, point.Y);

    public static Point3D<T> MoveBy<T>(this Point3D<T> point, Point3D<T> moveBy) where T : INumber<T> =>
        point + moveBy;

    public static T GetManhattenDistance<T>(this Point3D<T> point, Point3D<T> other) where T : INumber<T> =>
        T.Abs(point.X - other.X) + T.Abs(point.Y - other.Y) + T.Abs(point.Z - other.Z);
}