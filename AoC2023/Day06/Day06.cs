namespace AoC2023.Day06;

public class Day06 : IMDay
{
    public string FilePath { private get; init; } = "Day06\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (times, distancesToBeat) = await GetRaces();
        return Enumerable.Range(0, times!.Length)
            .Aggregate(1, (f, s) => f * GetWins(times[s], distancesToBeat![s]))
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (time, distanceToBeat) = await GetSingleRace();
        return GetWins((int)time, distanceToBeat).ToString();
    }

    public static int GetWins(int time, long distance)
    {
        var xTop = time / 2D;
        var yTop = xTop * xTop;
        var xDistance = (int)(Math.Sqrt(yTop - distance) * -1 + xTop);

        return time - xDistance * 2 - 1;
    }

    private async Task<int[][]> GetRaces() =>
        (await GetInput())
        .Select(l => l[1].ToIntArray(" "))
        .ToArray();

    private async Task<long[]> GetSingleRace() =>
        (await GetInput())
        .Select(l => l[1].Replace(" ", "").ParseToLong())
        .ToArray();

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadLinesAsStringArray(FilePath, ":");
}