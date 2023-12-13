namespace AoC2023.Day10;

public class Day10 : IMDay
{
    private Dictionary<PipeExit, (Point[] inner, Point[] outer)> _neighbors = new()
    {
        { new('F', new( 1,  0)), ([new(1, 1)], [new(-1, -1), new(-1, 0), new(-1, 1), new(0, -1), new(1, -1)]) },
        { new('|', new( 0,  1)), ([new(-1, -1), new(-1, 0), new(-1, 1)], [new(1, -1), new(1, 0), new(1, 1)]) },
        { new('J', new(-1,  0)), ([new(-1, -1)], [new(1, -1), new(1, 0), new(1, 1), new(-1, 1), new(0, 1)]) },
        { new('-', new(-1,  0)), ([new(-1, -1), new(0, -1), new(1, -1)], [new(-1, 1), new(0, 1), new(1, 1)]) },
        { new('L', new( 0, -1)), ([new(1, -1)], [new(-1, -1), new(-1, 0), new(-1, 1), new(0, 1), new(1, 1)]) },
        { new('7', new( 0,  1)), ([new(-1, 1)], [new(-1, -1), new(0, -1), new(1, -1), new(1, 0), new(1, 1)]) }
    };

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

    public Day10()
    {
        AddOtherExit('F', new( 1,  0), new( 0,  1));
        AddOtherExit('|', new( 0,  1), new( 0, -1));
        AddOtherExit('J', new(-1,  0), new( 0, -1));
        AddOtherExit('-', new(-1,  0), new( 1,  0));
        AddOtherExit('L', new( 0, -1), new( 1,  0));
        AddOtherExit('7', new( 0,  1), new(-1,  0));
    }

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        return (GetPipelinePipes(map).Count / 2).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = (await GetInput()).EnlargeMapByOneOnEachSide('.');
        var pipes = GetPipelinePipes(map);

        var pipePoints = pipes.Select(p => p.Point).ToList();
        var allOuters = pipes.SelectMany(p => p.Outer).Where(p => !pipePoints.Contains(p)).Distinct().ToList();
        var allInners = pipes.SelectMany(p => p.Inner).Where(p => !pipePoints.Contains(p)).Distinct().ToList();

        var openPoints = map.Where((p, v) => !allOuters.Contains(p) && !allInners.Contains(p) && !pipePoints.Contains(p));
        foreach (var open in openPoints)
        {
            if (map.GetStraightNeighbors(open).Any(allInners.Contains))
                allInners.Add(open);
            else if (map.GetStraightNeighbors(open).Any(allOuters.Contains))
                allOuters.Add(open);
        }

        openPoints = map.Where((p, v) => !allOuters.Contains(p) && !allInners.Contains(p) && !pipePoints.Contains(p)).Reverse();
        foreach (var open in openPoints)
        {
            if (map.GetStraightNeighbors(open).Any(allInners.Contains))
                allInners.Add(open);
            else if (map.GetStraightNeighbors(open).Any(allOuters.Contains))
                allOuters.Add(open);
        }

        if (allInners.Min(p => p.X) > allOuters.Min(p => p.X))
            return allInners.Count.ToString();
        else
            return allOuters.Count.ToString();
    }

    private List<Pipe> GetPipelinePipes(Map<char> map)
    {
        var start = map!.First((p, v) => v == 'S');
        List<Pipe> pipes = [new(start, 'S', [], [])];

        Pipe currentPipe = GetNextPipe(map, pipes[0], start);
        Point previousPoint = start;

        while (currentPipe.Point != start)
        {
            pipes.Add(currentPipe);
            var newPipe = GetNextPipe(map, currentPipe, previousPoint);
            previousPoint = currentPipe.Point;
            currentPipe = newPipe;
        }

        return pipes;
    }

    private Pipe GetNextPipe(Map<char> map, Pipe currentPipe, Point previousPoint)
    {
        var nextPoint = _pipes[currentPipe.Type]
            .Select(p => currentPipe.Point.Add(p))
            .First(p => p != previousPoint && map.GetValueOrDefault(p, '.') != '.' && _pipes[map.GetValue(p)].Any(d => p.Add(d) == currentPipe.Point));

        var pipeValue = map.GetValue(nextPoint);
        Point[] inner = [];
        Point[] outer = [];

        if (pipeValue != 'S')
        {
            var exit = _pipes[pipeValue].Single(p => nextPoint.Add(p) != currentPipe.Point);
            (inner, outer) = _neighbors[new PipeExit(pipeValue, exit)];
        }

        return new(
            nextPoint,
            pipeValue,
            inner.Select(p => nextPoint.Add(p)).Where(map.Contains).ToArray(),
            outer.Select(p => nextPoint.Add(p)).Where(map.Contains).ToArray());
    }

    private void AddOtherExit(char type, Point exit, Point otherExit)
    {
        var (inner, outer) = _neighbors[new(type, exit)];
        _neighbors.Add(new(type, otherExit), (outer, inner));
    }

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));

    private record struct Pipe(Point Point, char Type, Point[] Inner, Point[] Outer);
    private record struct PipeExit(char Type, Point Exit);
}
