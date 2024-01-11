namespace AoC2023.Day17;

public class Day17 : IMDay
{
    public string FilePath { private get; init; } = "Day17\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();

        var result = GetShortestPath(
            map,
            new(0, 0),
            new(map.SizeX - 1, map.SizeY - 1),
            GetValidNeighborsPart1,
            _ => true);

        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();

        var result = GetShortestPath(
            map,
            new(0, 0),
            new(map.SizeX - 1, map.SizeY - 1),
            GetValidNeighborsPart2,
            p => p.Steps > 3);

        return result.ToString();
    }

    private static int GetShortestPath(Map<int> map, Point from, Point to, Func<Map<int>, WeightedPoint, IEnumerable<WeightedPoint>> getNeighbors, Func<WeightedPoint, bool> isValidEnd)
    {
        WeightedPoint startRight = new(from, 0, new(1, 0));
        WeightedPoint startDown = new(from, 0, new(0, 1));

        Dictionary<WeightedPoint, int> currentCostPerPoint = new() { { startRight, 0 }, { startDown, 0 } };
        HashSet<WeightedPoint> visitedPoints = [];
        PriorityQueue<WeightedPoint, int> openPositions = new();

        openPositions.Enqueue(startRight, 0);
        openPositions.Enqueue(startDown, 0);

        var totalCost = -1;

        while (totalCost == -1)
        {
            var point = openPositions.Dequeue();
            visitedPoints.Add(point);
            var cost = currentCostPerPoint[point];

            var nextPoints = getNeighbors(map, point)
                .Where(n => !visitedPoints.Contains(n));

            foreach (var next in nextPoints)
            {
                var nextCost = map.GetValue(next.Point) + cost;

                if (next.Point == to && isValidEnd(next))
                {
                    totalCost = nextCost;
                    break;
                }

                if (!currentCostPerPoint.TryGetValue(next, out var currentCost) || nextCost < currentCost)
                {
                    currentCostPerPoint.AddOrSet(next, nextCost);
                    openPositions.Enqueue(next, nextCost);
                }
            }
        }

        return totalCost;
    }

    private static IEnumerable<WeightedPoint> GetValidNeighborsPart1(Map<int> map, WeightedPoint point) =>
        map.GetStraightNeighbors(point.Point)
            .Where(n => point.Point.Subtract(n) != point.Direction)
            .Select(n => new WeightedPoint(n, n.Subtract(point.Point) == point.Direction ? point.Steps + 1 : 1, n.Subtract(point.Point)))
            .Where(n => n.Steps < 4);

    private static IEnumerable<WeightedPoint> GetValidNeighborsPart2(Map<int> map, WeightedPoint point) =>
        map.GetStraightNeighbors(point.Point)
            .Where(n => point.Point.Subtract(n) != point.Direction)
            .Select(n => new WeightedPoint(n, n.Subtract(point.Point) == point.Direction ? point.Steps + 1 : 1, n.Subtract(point.Point)))
            .Where(n => (point.Steps >= 4 || point.Direction == n.Direction) && n.Steps <= 10);

    private async Task<Map<int>> GetInput() =>
        new(await FileParser.ReadLinesAsIntArray(FilePath));

    private record struct WeightedPoint(Point Point, int Steps, Point Direction);
}
