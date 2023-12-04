namespace AoC2023.Day03;

public class Day03 : IMDay
{
    public string FilePath { private get; init; } = "Day03\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();

        return map
            .Where((p, v) => IsNumberWithSymbolNeighbor(p, v, map))
            .Select(map.GetSpannedInt)
            .Distinct()
            .Sum(n => n.Value)
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();

        return map
            .Where((p, v) => IsGearWithNumberNeighbor(p, v, map))
            .Select(g => map.GetStraightAndDiagonalNeighbors(g).Where(p => map.GetValueOrDefault(p, '.').IsNumber()))
            .Select(g => g.Select(map.GetSpannedInt).Distinct().Select(v => v.Value))
            .Select(g => g.Count() > 1 ? g.Aggregate((f, s) => f * s) : 0)
            .Sum()
            .ToString();
    }

    private static bool IsNumberWithSymbolNeighbor(Point point, char value, Map<char> map) =>
        value.IsNumber() && map.GetStraightAndDiagonalNeighbors(point).Any(n => map.GetValue(n) != '.' && !map.GetValue(n).IsNumber());

    private static bool IsGearWithNumberNeighbor(Point point, char value, Map<char> map) =>
        value == '*' && map.GetStraightAndDiagonalNeighbors(point).Any(n => map.GetValue(n).IsNumber());

    private async Task<Map<char>> GetInput() =>
        new Map<char>(await FileParser.ReadLinesAsCharArray(FilePath));
}
