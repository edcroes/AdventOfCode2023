namespace AoC.Common.Geometry;

public readonly struct Line3D(Point3D from, Point3D to) : IEquatable<Line3D>
{
    public Point3D From { get; } = new(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y), Math.Min(from.Z, to.Z));
    public Point3D To { get; } = new(Math.Max(from.X, to.X), Math.Max(from.Y, to.Y), Math.Max(from.Z, to.Z));

    public long CubeCount =>
        (To.X - From.X + 1L) *
        (To.Y - From.Y + 1L) *
        (To.Z - From.Z + 1L);

    public static Line3D Empty => new(Point3D.Empty, Point3D.Empty);

    public Line3D Intersect(Line3D other)
    {
        if (!HasOverlapWith(other))
            return Empty;

        Point3D from = new(
            Math.Max(From.X, other.From.X),
            Math.Max(From.Y, other.From.Y),
            Math.Max(From.Z, other.From.Z)
        );
        Point3D to = new(
            Math.Min(To.X, other.To.X),
            Math.Min(To.Y, other.To.Y),
            Math.Min(To.Z, other.To.Z)
        );

        return new(from, to);
    }

    public bool HasOverlapWith(Line3D other) =>
        From.X <= other.To.X && To.X >= other.From.X &&
        From.Y <= other.To.Y && To.Y >= other.From.Y &&
        From.Z <= other.To.Z && To.Z >= other.From.Z;

    public bool HasOverlapOnXAndYWith(Line3D other) =>
        From.X <= other.To.X && To.X >= other.From.X &&
        From.Y <= other.To.Y && To.Y >= other.From.Y;

    public bool Contains(Line3D other) =>
        From.X <= other.From.X && To.X >= other.To.X &&
        From.Y <= other.From.Y && To.Y >= other.To.Y &&
        From.Z <= other.From.Z && To.Z >= other.To.Z;

    public List<Line3D> Explode(Line3D other)
    {
        if (!HasOverlapWith(other))
            return [this];

        if (!Contains(other))
            other = Intersect(other);

        if (this == other)
            return [];

        List<Line3D> subCuboids = [];
        var remainder = this;

        if (remainder.From.X < other.From.X)
        {
            (var left, remainder) = remainder.SliceLeftOfX(other.From.X);
            subCuboids.Add(left);
        }

        if (other.To.X < remainder.To.X)
        {
            (remainder, var right) = remainder.SliceLeftOfX(other.To.X + 1);
            subCuboids.Add(right);
        }

        if (remainder.From.Y < other.From.Y)
        {
            (var left, remainder) = remainder.SliceLeftOfY(other.From.Y);
            subCuboids.Add(left);
        }

        if (other.To.Y < remainder.To.Y)
        {
            (remainder, var right) = remainder.SliceLeftOfY(other.To.Y + 1);
            subCuboids.Add(right);
        }

        if (remainder.From.Z < other.From.Z)
        {
            (var left, remainder) = remainder.SliceLeftOfZ(other.From.Z);
            subCuboids.Add(left);
        }

        if (other.To.Z < remainder.To.Z)
        {
            (remainder, var right) = remainder.SliceLeftOfZ(other.To.Z + 1);
            subCuboids.Add(right);
        }

        if (remainder != other)
            throw new InvalidOperationException("That sucks... the remaining cube should be the cube to explode");

        return subCuboids;
    }

    public (Line3D, Line3D) SliceLeftOfX(int x)
    {
        Point3D leftTo = new(x - 1, To.Y, To.Z);
        Line3D left = new(From, leftTo);

        Point3D rightFrom = new(x, From.Y, From.Z);
        Line3D right = new(rightFrom, To);

        return (left, right);
    }

    public (Line3D, Line3D) SliceLeftOfY(int y)
    {
        Point3D leftTo = new(To.X, y - 1, To.Z);
        Line3D left = new(From, leftTo);

        Point3D rightFrom = new(From.X, y, From.Z);
        Line3D right = new(rightFrom, To);

        return (left, right);
    }

    public (Line3D, Line3D) SliceLeftOfZ(int z)
    {
        Point3D leftTo = new(To.X, To.Y, z - 1);
        Line3D left = new(From, leftTo);

        Point3D rightFrom = new(From.X, From.Y, z);
        Line3D right = new(rightFrom, To);

        return (left, right);
    }

    public override bool Equals(object? obj) =>
        obj is Line3D cube && Equals(cube);

    public bool Equals(Line3D other) =>
        From == other.From && To == other.To;

    public override int GetHashCode() =>
        HashCode.Combine(From, To);

    public static bool operator ==(Line3D left, Line3D right) =>
        left.Equals(right);

    public static bool operator !=(Line3D left, Line3D right) =>
        !left.Equals(right);
}