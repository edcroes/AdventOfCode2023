namespace AoC2023.Day22;

public class Day22 : IMDay
{
    private Dictionary<Line3D<int>, List<Line3D<int>>>? _supporting = null;
    private Dictionary<Line3D<int>, Line3D<int>[]>? _supportedBy = null;

    public string FilePath { private get; init; } = "Day22\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var lines = await GetInput();
        if (_supporting is null || _supportedBy is null)
            (_supporting, _supportedBy) = GetStack(lines);

        return _supportedBy.Keys.Count(k => _supportedBy.Keys.Where(o => _supportedBy[o].Contains(k)).All(o => _supportedBy[o].Length > 1)).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var cuboids = await GetInput();
        if (_supporting is null || _supportedBy is null)
            (_supporting, _supportedBy) = GetStack(cuboids);

        var totalFallen = 0;
        foreach (var brick in _supportedBy.Keys)
        {
            HashSet<Line3D<int>> fallen = [brick];
            LetBrickFall(brick, fallen);
            totalFallen += fallen.Count - 1;
        }

        return totalFallen.ToString();
    }

    private void LetBrickFall(Line3D<int> line, HashSet<Line3D<int>> fallen)
    {
        if (!_supporting!.TryGetValue(line, out var supported))
            return;

        var nextThatWillFall = supported.Where(n => _supportedBy![n].All(s => fallen.Contains(s))).ToArray();
        fallen.AddRange(nextThatWillFall);

        foreach (var next in nextThatWillFall)
            LetBrickFall(next, fallen);
    }

    private static (Dictionary<Line3D<int>, List<Line3D<int>>> supporting, Dictionary<Line3D<int>, Line3D<int>[]> supportedBy) GetStack(IEnumerable<Line3D<int>> lines)
    {
        HashSet<Line3D<int>> fallen = [new(new(-1, -1, -1), new(-1, -1, -1))]; // Add dummy so that Max() won't complain
        Dictionary<Line3D<int>, List<Line3D<int>>> supporting = [];
        Dictionary<Line3D<int>, Line3D<int>[]> supportedBy = [];

        foreach (var line in lines.OrderBy(c => c.From.Z))
        {
            var newZ = 1 + fallen.Max(l => l.HasOverlapOnXAndYWith(line) ? l.To.Z : 0);
            Line3D<int> newLine = new(new(line.From.X, line.From.Y, newZ), new(line.To.X, line.To.Y, newZ + (line.To.Z - line.From.Z)));

            var lower = fallen
                .Where(l => l.HasOverlapOnXAndYWith(newLine) && l.To.Z + 1 == newLine.From.Z)
                .ToArray();

            foreach (var support in lower)
                supporting.AddOrUpdate(support, newLine);

            supportedBy.Add(newLine, lower);
            fallen.Add(newLine);
        }

        return (supporting, supportedBy);
    }

    private async Task<Line3D<int>[]> GetInput() =>
        (await FileParser.ReadLinesAsStringArray(FilePath, "~"))
        .Select(l => l.Select(p =>
        {
            var (x, y, z) = p.Split(",");
            return new Point3D<int>(x!.ParseToInt(), y!.ParseToInt(), z!.ParseToInt());
        }).ToArray())
        .Select(l => new Line3D<int>(l[0], l[1]))
        .ToArray();
}
