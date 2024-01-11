namespace AoC2023.Day23;

public class Day23 : IMDay
{
    public string FilePath { private get; init; } = "Day23\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();

        Point start = new(1, 0);
        Point end = new(map.SizeX - 2, map.SizeY - 1);

        return map.GetLongestPath(start, end, GetValidNeighbors).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        Point start = new(1, 0);
        Point end = new(map.SizeX - 2, map.SizeY - 1);

        var graph = map.ToWeightedGraph(start, ['.', '>', 'v'], GraphStrategy.KeepLongestPath);

        return graph.GetLongestPath(start, end).ToString();
    }

    private static IEnumerable<Point> GetValidNeighbors(Map<char> map, Point current) =>
        map.GetStraightNeighbors(current)
            .Where(n => IsValidMove(map.GetValue(n), n.Subtract(current)));

    private static bool IsValidMove(char value, Point direction) =>
        value == '.' || (direction == PointExtensions.Right && value == '>') || (direction == PointExtensions.Down && value == 'v');

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
