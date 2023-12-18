using AoC.Common.Maps;

namespace AoC2023.Day10;

public class Day10 : IMDay
{
    private static readonly Dictionary<char, Point[]> _pipes = new()
    {
        { 'F', new Point[] { new(1, 0), new(0, 1) } },
        { '|', new Point[] { new(0, -1), new(0, 1) } },
        { 'J', new Point[] { new(0, -1), new(-1, 0) } },
        { '-', new Point[] { new(-1 ,0), new(1, 0)} },
        { 'L', new Point[] { new(0, -1), new(1, 0) } },
        { '7', new Point[] { new(-1, 0), new(0, 1) } },
        { 'S', new Point[] { new(-1, 0), new(1, 0), new(0, -1), new(0, 1) } }
    };

    public string FilePath { private get; init; } = "Day10\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        return (GetPipelinePipes(map).Count / 2).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = (await GetInput()).EnlargeMapByOneOnEachSide('.');
        var pipes = GetPipelinePipes(map);

        map.FillShape(pipes, 'P', '#');
        return map.Count('#').ToString();
    }

    private List<Point> GetPipelinePipes(Map<char> map)
    {
        var start = map!.First((p, v) => v == 'S');
        List<Point> pipes = [start];

        var currentPipe = GetNextPipe(map, pipes[0], start);
        Point previousPipe = start;

        while (currentPipe != start)
        {
            pipes.Add(currentPipe);
            var newPipe = GetNextPipe(map, currentPipe, previousPipe);
            previousPipe = currentPipe;
            currentPipe = newPipe;
        }

        return pipes;
    }

    private static Point GetNextPipe(Map<char> map, Point currentPipe, Point previousPipe) =>
        _pipes[map.GetValue(currentPipe)]
            .Select(p => currentPipe.Add(p))
            .First(p => p != previousPipe && map.GetValueOrDefault(p, '.') != '.' && _pipes[map.GetValue(p)].Any(d => p.Add(d) == currentPipe));

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
