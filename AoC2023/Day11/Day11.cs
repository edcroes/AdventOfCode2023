namespace AoC2023.Day11;

public class Day11 : IMDay
{
    public string FilePath { private get; init; } = "Day11\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        map = EnlargeMap(map);

        return Enumerable.Range(0, map.Points.Count)
            .Aggregate(0L, (f, s) => f + GetDistanceToAllPoints(map.Points[s], map.Points.Skip(s)))
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        map = EnlargeMap(map, 1000000 - 1);

        return Enumerable.Range(0, map.Points.Count)
            .Aggregate(0L, (f, s) => f + GetDistanceToAllPoints(map.Points[s], map.Points.Skip(s)))
            .ToString();
    }

    private static long GetDistanceToAllPoints(Point<long> point, IEnumerable<Point<long>> others) =>
        others.Aggregate(0L, (f, s) => f + s.GetManhattenDistance(point));

    private static PointMap<long, bool> EnlargeMap(PointMap<long, bool> map, long times = 1)
    {
        var mapRectangle = map.GetBoundingRectangle();
        var emptyY = Enumerable.Range((int)mapRectangle.Y, (int)mapRectangle.Height)
            .Where(y => map.Points.All(p => p.Y != y))
            .Select(i => (long)i)
            .ToArray();
        var emptyX = Enumerable.Range((int)mapRectangle.X, (int)mapRectangle.Width)
            .Where(x => map.Points.All(p => p.X != x))
            .Select(i => (long)i)
            .ToArray();

        PointMap<long, bool> newMap = new();
        var newPoints = map.Points.Select(p => new Point<long>(
            emptyX.Count(e => e < p.X) * times + p.X,
            emptyY.Count(e => e < p.Y) * times + p.Y)
        );
        
        foreach (var point in newPoints)
            newMap.SetValue(point, true);

        return newMap;
    }

    private async Task<PointMap<long, bool>> GetInput()
    {
        var tempMap = new Map<char>(await FileParser.ReadLinesAsCharArray(FilePath));
        PointMap<long, bool> map = new();
        foreach (var point in tempMap.Where((p, v) => v == '#'))
            map.SetValue(point.X, point.Y, true);

        return map;
    }
}
