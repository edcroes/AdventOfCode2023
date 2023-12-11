using System.Numerics;

namespace AoC.Common.Maps;

public class PointMap<TPointType, TValue> where TPointType : INumber<TPointType>
{
    private readonly Dictionary<Point<TPointType>, TValue> _points = [];

    public PointMap() { }

    public PointMap(TValue[][] values, bool insertDefaultValue = false)
    {
        for (var y = 0; y < values.Length; y++)
        {
            for (var x = 0; x < values.Min(l => l.Length); x++)
            {
                if ((values[y][x] != null && !values[y][x]!.Equals(default)) || insertDefaultValue)
                {
                    SetValue(new(TPointType.CreateChecked(x), TPointType.CreateChecked(y)), values[y][x]);
                }
            }
        }
    }

    public IReadOnlyList<Point<TPointType>> Points =>
        _points.Keys.ToList();

    public TValue GetValue(TPointType x, TPointType y) =>
        GetValue(new(x, y));

    public TValue GetValue(Point<TPointType> point) =>
        _points.TryGetValue(point, out TValue? value) ? value : throw new ArgumentOutOfRangeException(nameof(point));

    public TValue? GetValueOrDefault(TPointType x, TPointType y, TValue defaultValue = default) =>
        GetValueOrDefault(new(x, y), defaultValue);

    public TValue? GetValueOrDefault(Point<TPointType> point, TValue defaultValue = default) =>
        _points.TryGetValue(point, out TValue? value) ? value : defaultValue;

    public void SetValue(TPointType x, TPointType y, TValue value) =>
        SetValue(new(x, y), value);

    public void SetValue(Point<TPointType> point, TValue value) =>
        _points.AddOrSet(point, value);

    public void RemoveValue(Point<TPointType> point) =>
        _points.Remove(point);

    public IEnumerable<Point<TPointType>> GetStraightAndDiagonalNeighbors(Point<TPointType> point)
    {
        List<Point<TPointType>> neighbors = [];

        for (var y = point.Y - TPointType.One; y <= point.Y + TPointType.One; y++)
        {
            for (var x = point.X - TPointType.One; x <= point.X + TPointType.One; x++)
            {
                if (y == point.Y && x == point.X)
                {
                    continue;
                }

                neighbors.Add(new Point<TPointType>(x, y));
            }
        }

        return neighbors;
    }

    public int NumberOfStraightAndDiagonalNeighborsThatMatch(Point<TPointType> point, TValue valueToMatch) =>
        NumberOfStraightAndDiagonalNeighborsThatMatch(point, p => _points.TryGetValue(p, out TValue? value) && valueToMatch.Equals(value));

    public int NumberOfStraightAndDiagonalNeighborsThatMatch(Point<TPointType> point, Func<Point<TPointType>, bool> getMatch) =>
        GetStraightAndDiagonalNeighbors(point).Count(getMatch);

    public Rectangle<TPointType> GetBoundingRectangle()
    {
        if (_points.Keys.Count == 0)
            throw new InvalidOperationException("Unable to determine the bounding rectangle of a map without points");

        var minX = _points.Keys.Min(p => p.X);
        var maxX = _points.Keys.Max(p => p.X);
        var minY = _points.Keys.Min(p => p.Y);
        var maxY = _points.Keys.Max(p => p.Y);

        return new(minX!, minY!, maxX! - minX!, maxY! - minY!);
    }
}
