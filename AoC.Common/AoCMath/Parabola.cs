namespace AoC.Common.AoCMath;

public class Parabola
{
    /// <summary>
    /// Vertex is also known as Top
    /// </summary>
    public Point<double> Vertex { get; }

    public ParabolaType Type { get; }

    public double A { get; }

    public Parabola(Point<double> vertex, Point<double> otherKnownPoint, ParabolaType type = ParabolaType.OpensVertically)
    {
        Vertex = vertex;
        Type = type;
        A = GetA(otherKnownPoint);
    }

    public double GetYForX(double X)
    {
        if (Type == ParabolaType.OpensHorizontally)
        {
            var lastY = Math.Sqrt((X - Vertex.X) / A) + Vertex.Y;
            return Vertex.Y - (lastY - Vertex.Y);
        }

        return A * Math.Pow(X - Vertex.X, 2) + Vertex.Y;
    }

    public double GetXForY(double Y)
    {
        if (Type == ParabolaType.OpensVertically)
        {
            var lastX = Math.Sqrt((Y - Vertex.Y) / A) + Vertex.X;
            return Vertex.X - (lastX - Vertex.X);
        }

        return A * Math.Pow(Y - Vertex.Y, 2) + Vertex.X;
    }

    private double GetA(Point<double> knownValue)
    {
        if (Type == ParabolaType.OpensVertically)
            return (knownValue.Y - Vertex.Y) / Math.Pow(knownValue.X - Vertex.X, 2);

        return (knownValue.X - Vertex.X) / Math.Pow(knownValue.Y - Vertex.Y, 2);
    }

    public enum ParabolaType
    {
        OpensVertically   = 0,
        OpensHorizontally = 1
    }
}
