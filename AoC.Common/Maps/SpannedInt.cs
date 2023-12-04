namespace AoC.Common.Maps;

public record struct SpannedInt(Point Start, int Value)
{
    public readonly int Length => Value.GetDigitCount();

    public static implicit operator int(SpannedInt value) => value.Value;
}