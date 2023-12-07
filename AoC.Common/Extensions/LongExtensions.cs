namespace AoC.Common.Extensions;

public static class LongExtensions
{
    public static int GetNthDigit(this long value, int n) =>
        (int)(value / (long)Math.Pow(10, n) % 10);

    public static long SetNthDigit(this long value, int n, int to) =>
         value + (to - value.GetNthDigit(n)) * (long)Math.Pow(10, n);

    public static bool IsEven(this long value) =>
        value % 2 == 0;
}