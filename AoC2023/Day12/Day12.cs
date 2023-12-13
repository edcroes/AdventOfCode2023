namespace AoC2023.Day12;

public class Day12 : IMDay
{
    public string FilePath { private get; init; } = "Day12\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (damagedMap, map) = await GetDamagedMap();

        return Enumerable.Range(0, damagedMap.Length)
            .Sum(i => GetNumberOfArrangements(damagedMap[i], map[i]))
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (damagedMap, map) = await GetDamagedMap();

        return Enumerable.Range(0, damagedMap.Length)
            .Select(i => (
                $"{damagedMap[i]}?{damagedMap[i]}?{damagedMap[i]}?{damagedMap[i]}?{damagedMap[i]}",
                Enumerable.Repeat(map[i], 5).SelectMany(i => i).ToArray()
            ))
            .Sum(i => GetNumberOfArrangements(i.Item1, i.Item2))
            .ToString();
    }

    private static long GetNumberOfArrangements(string damagedMap, int[] map, int damagedMapIndex = 0, int current = 0, Dictionary<Point, long>? cache = null)
    {
        cache ??= [];

        if (cache.TryGetValue(new(damagedMapIndex, current), out long cachedResult))
            return cachedResult;

        var result = 0L;

        for (var i = damagedMapIndex; i <= damagedMap.Length - map[current]; i++)
        {
            var part = damagedMap[i..(map[current] + i)];

            if (!part.Contains('.'))
            {
                var next = i + map[current];
                var isLast = current == map.Length - 1;
                if (isLast && !damagedMap[next..].Contains('#'))
                {
                    result++;
                }
                else if (!isLast && next + 1 < damagedMap.Length && damagedMap[next] != '#')
                {
                    result += GetNumberOfArrangements(damagedMap, map, next + 1, current + 1, cache);
                }
            }

            if (damagedMap[i] == '#')
                break;
        }

        cache.Add(new(damagedMapIndex, current), result);
        return result;
    }

    private async Task<(string[] damagedMap, int[][] map)> GetDamagedMap()
    {
        var input = await GetInput();
        var map = input.Select(i => i[1].ToIntArray(",")).ToArray();

        return (input.Select(i => i[0]).ToArray(), map);
    }

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadLinesAsStringArray(FilePath, " ");
}
