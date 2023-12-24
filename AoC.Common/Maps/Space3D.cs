using System.Numerics;

namespace AoC.Common.Maps;

public class Space3D<T> where T : INumber<T>
{
    private readonly List<Point3D<T>> _points = new();
    private int[] _cornerPointIndeces = [];

    public string Name { get; init; } = "unnamed";

    public Point3D<T> Center { get; private set; } = new(T.Zero, T.Zero, T.Zero);

    public Space3D() { }

    public Space3D(IEnumerable<Point3D<T>> points)
    {
        AddRange(points);
    }

    public IReadOnlyList<Point3D<T>> Points => _points;
    public IReadOnlyList<Point3D<T>> CornerPoints => _cornerPointIndeces.Select(i => _points[i]).ToList();

    public void Add(Point3D<T> point)
    {
        AddPoint(point);
        ReindexCorners();
    }

    public void AddRange(IEnumerable<Point3D<T>> points)
    {
        foreach (var point in points)
        {
            AddPoint(point);
        }

        ReindexCorners();
    }

    public Space3D<T> RotateX()
    {
        for (var i = 0; i < _points.Count; i++)
        {
            _points[i] = _points[i].RotateX();
        }
        Center = Center.RotateX();

        return this;
    }

    public Space3D<T> RotateY()
    {
        for (var i = 0; i < _points.Count; i++)
        {
            _points[i] = _points[i].RotateY();
        }
        Center = Center.RotateY();

        return this;
    }

    public Space3D<T> RotateZ()
    {
        for (var i = 0; i < _points.Count; i++)
        {
            _points[i] = _points[i].RotateZ();
        }
        Center = Center.RotateZ();

        return this;
    }

    public Space3D<T> MoveBy(Point3D<T> moveBy)
    {
        for (var i = 0; i < _points.Count; i++)
        {
            _points[i] = _points[i].MoveBy(moveBy);
        }
        Center = Center.MoveBy(moveBy);

        return this;
    }

    private void AddPoint(Point3D<T> point)
    {
        if (!_points.Contains(point))
        {
            _points.Add(point);
        }
    }

    private void ReindexCorners()
    {
        var cornerPoints = new[] {
            _points.Where(p => (p - Center).X < T.Zero && (p - Center).Y < T.Zero && (p - Center).Z < T.Zero).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X < T.Zero && (p - Center).Y < T.Zero && (p - Center).Z > T.Zero).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X < T.Zero && (p - Center).Y > T.Zero && (p - Center).Z < T.Zero).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X > T.Zero && (p - Center).Y < T.Zero && (p - Center).Z < T.Zero).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X < T.Zero && (p - Center).Y > T.Zero && (p - Center).Z > T.Zero).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X > T.Zero && (p - Center).Y < T.Zero && (p - Center).Z > T.Zero).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X > T.Zero && (p - Center).Y > T.Zero && (p - Center).Z < T.Zero).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault(),
            _points.Where(p => (p - Center).X > T.Zero && (p - Center).Y > T.Zero && (p - Center).Z > T.Zero).OrderBy(p => p.GetManhattenDistance(Center)).LastOrDefault()
        };

        _cornerPointIndeces = cornerPoints
            .Where(p => p != default)
            .Select(p => _points.IndexOf(p))
            .ToArray();
    }
}