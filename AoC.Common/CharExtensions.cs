namespace AoC.Common;

public static class CharExtensions
{
    public static bool IsNumber(this char value) => char.IsNumber(value);

    public static bool IsDigit(this char value) => char.IsDigit(value);

    public static int ToNumber(this char value) => value - 48;
}
