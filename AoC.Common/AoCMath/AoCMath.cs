using AoC.Common.Geometry;

namespace AoC.Common.AoCMath;

public static class AoCMath
{
    public static long Factorial(int value)
    {
        var fact = 1L;
        for (int next = value; next > 0; next--)
        {
            fact *= next;
        }
        return fact;
    }

    public static long GreatestCommonDivisor(long a, long b)
    {
        if (a < 0 || b < 0)
        {
            throw new ArgumentException("Both a and b should be greater than or equal to 0");
        }

        if (a == 0)
        {
            return b;
        }

        while (b != 0)
        {
            var newA = b;
            b = a % b;
            a = newA;
        }

        return a;
    }

    public static long GreatestCommonDivisor(IEnumerable<long> values) =>
        values.Aggregate(GreatestCommonDivisor);

    public static long LeastCommonMultiple(long a, long b) =>
        a * b / GreatestCommonDivisor(a, b);

    public static long LeastCommonMultiple(IEnumerable<long> values) =>
        values.Aggregate(LeastCommonMultiple);

    public static double GetPolygonAreaWithBorder(IEnumerable<Point> verticesAntiClockwise)
    {
        var vertices = verticesAntiClockwise.ToArray();
        var sum1 = 0L;
        var sum2 = 0L;
        var borderLength = 0D;

        for (var i = 0; i < vertices.Length; i++)
        {
            var next = i == vertices.Length - 1 ? vertices[0] : vertices[i + 1];
            sum1 += (long)vertices[i].X * next.Y;
            sum2 += (long)vertices[i].Y * next.X;
            borderLength += new Line(vertices[i], next).Length;
        }

        return (double)Math.Abs(sum1 - sum2) / 2 + borderLength / 2 + 1;
    }

    public static double GetPolygonArea(IEnumerable<Point> verticesAntiClockwise)
    {
        var vertices = verticesAntiClockwise.ToArray();
        var sum1 = 0L;
        var sum2 = 0L;

        for (var i = 0; i < vertices.Length; i++)
        {
            var next = i == vertices.Length - 1 ? vertices[0] : vertices[i + 1];
            sum1 += (long)vertices[i].X * next.Y;
            sum2 += (long)vertices[i].Y * next.X;
        }

        return (double)Math.Abs(sum1 - sum2) / 2;
    }
}