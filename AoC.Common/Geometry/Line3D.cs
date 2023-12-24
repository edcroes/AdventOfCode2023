using System.Numerics;

namespace AoC.Common.Geometry;

public readonly struct Line3D<T>(Point3D<T> from, Point3D<T> to) : IEquatable<Line3D<T>> where T : INumber<T>
{
    public Point3D<T> From { get; } = new(T.Min(from.X, to.X), T.Min(from.Y, to.Y), T.Min(from.Z, to.Z));
    public Point3D<T> To { get; } = new(T.Max(from.X, to.X), T.Max(from.Y, to.Y), T.Max(from.Z, to.Z));

    public T CubeCount =>
        (To.X - From.X + T.One) *
        (To.Y - From.Y + T.One) *
        (To.Z - From.Z + T.One);

    public static Line3D<T> Empty => new(Point3D<T>.Empty, Point3D<T>.Empty);

    public Line3D<T> Intersect(Line3D<T> other)
    {
        if (!HasOverlapWith(other))
            return Empty;

        Point3D<T> from = new(
            T.Max(From.X, other.From.X),
            T.Max(From.Y, other.From.Y),
            T.Max(From.Z, other.From.Z)
        );
        Point3D<T> to = new(
            T.Min(To.X, other.To.X),
            T.Min(To.Y, other.To.Y),
            T.Min(To.Z, other.To.Z)
        );

        return new(from, to);
    }

    public bool HasOverlapWith(Line3D<T> other) =>
        From.X <= other.To.X && To.X >= other.From.X &&
        From.Y <= other.To.Y && To.Y >= other.From.Y &&
        From.Z <= other.To.Z && To.Z >= other.From.Z;

    public bool HasOverlapOnXAndYWith(Line3D<T> other) =>
        From.X <= other.To.X && To.X >= other.From.X &&
        From.Y <= other.To.Y && To.Y >= other.From.Y;

    public bool Contains(Line3D<T> other) =>
        From.X <= other.From.X && To.X >= other.To.X &&
        From.Y <= other.From.Y && To.Y >= other.To.Y &&
        From.Z <= other.From.Z && To.Z >= other.To.Z;

    public List<Line3D<T>> Explode(Line3D<T> other)
    {
        if (!HasOverlapWith(other))
            return [this];

        if (!Contains(other))
            other = Intersect(other);

        if (this == other)
            return [];

        List<Line3D<T>> subCuboids = [];
        var remainder = this;

        if (remainder.From.X < other.From.X)
        {
            (var left, remainder) = remainder.SliceLeftOfX(other.From.X);
            subCuboids.Add(left);
        }

        if (other.To.X < remainder.To.X)
        {
            (remainder, var right) = remainder.SliceLeftOfX(other.To.X + T.One);
            subCuboids.Add(right);
        }

        if (remainder.From.Y < other.From.Y)
        {
            (var left, remainder) = remainder.SliceLeftOfY(other.From.Y);
            subCuboids.Add(left);
        }

        if (other.To.Y < remainder.To.Y)
        {
            (remainder, var right) = remainder.SliceLeftOfY(other.To.Y + T.One);
            subCuboids.Add(right);
        }

        if (remainder.From.Z < other.From.Z)
        {
            (var left, remainder) = remainder.SliceLeftOfZ(other.From.Z);
            subCuboids.Add(left);
        }

        if (other.To.Z < remainder.To.Z)
        {
            (remainder, var right) = remainder.SliceLeftOfZ(other.To.Z + T.One);
            subCuboids.Add(right);
        }

        if (remainder != other)
            throw new InvalidOperationException("That sucks... the remaining cube should be the cube to explode");

        return subCuboids;
    }

    public (Line3D<T>, Line3D<T>) SliceLeftOfX(T x)
    {
        Point3D<T> leftTo = new(x - T.One, To.Y, To.Z);
        Line3D<T> left = new(From, leftTo);

        Point3D<T> rightFrom = new(x, From.Y, From.Z);
        Line3D<T> right = new(rightFrom, To);

        return (left, right);
    }

    public (Line3D<T>, Line3D<T>) SliceLeftOfY(T y)
    {
        Point3D<T> leftTo = new(To.X, y - T.One, To.Z);
        Line3D<T> left = new(From, leftTo);

        Point3D<T> rightFrom = new(From.X, y, From.Z);
        Line3D<T> right = new(rightFrom, To);

        return (left, right);
    }

    public (Line3D<T>, Line3D<T>) SliceLeftOfZ(T z)
    {
        Point3D<T> leftTo = new(To.X, To.Y, z - T.One);
        Line3D<T> left = new(From, leftTo);

        Point3D<T> rightFrom = new(From.X, From.Y, z);
        Line3D<T> right = new(rightFrom, To);

        return (left, right);
    }

    public override bool Equals(object? obj) =>
        obj is Line3D<T> cube && Equals(cube);

    public bool Equals(Line3D<T> other) =>
        From == other.From && To == other.To;

    public override int GetHashCode() =>
        HashCode.Combine(From, To);

    public static bool operator ==(Line3D<T> left, Line3D<T> right) =>
        left.Equals(right);

    public static bool operator !=(Line3D<T> left, Line3D<T> right) =>
        !left.Equals(right);
}