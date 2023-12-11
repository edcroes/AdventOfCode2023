namespace AoC2023.Day09;

public class Day09 : IMDay
{
    public string FilePath { private get; init; } = "Day09\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();

        return input.Sum(GetNextValue).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();

        return input.Sum(i => GetNextValue(i.Reverse().ToArray())).ToString();
    }

    private static int GetNextValue(int[] history)
    {
        List<int[]> differences = [history];

        var next = history;
        while (!next.All(v => v == 0))
        {
            next = next.Skip(1).Zip(next, (f, s) => f - s).ToArray();
            differences.Add(next);
        }

        differences.Reverse();
        return differences.Aggregate(0, (f, s) => f + s.Last());
    }

    private async Task<int[][]> GetInput() =>
        await FileParser.ReadLinesAsIntArray(FilePath, " ");
}
