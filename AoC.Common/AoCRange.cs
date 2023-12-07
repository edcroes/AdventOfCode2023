namespace AoC.Common;

public record struct AoCRange(long Start, long Length)
{
    public readonly long End => Start + Length - 1;

    public readonly bool Contains(long value) =>
        value >= Start && value <= End;

    public readonly bool Contains(AoCRange other) =>
        other.Start >= Start && other.End <= End;

    public readonly bool HasOverlap(AoCRange other) =>
        (other.Start >= Start && other.Start <= End) ||
        (other.End >= Start && other.End <= End) ||
        (other.Start < Start && other.End > End) ||
        (Start < other.Start && End > other.End);

    public readonly AoCRange Intersect(AoCRange other)
    {
        if (!HasOverlap(other))
            throw new ArgumentException("Intersect is only possible of the two ranges overlap");

        var newStart = Math.Max(Start, other.Start);
        var newEnd = Math.Min(End, other.End);
        return new(newStart, newEnd - newStart + 1);
    }
}