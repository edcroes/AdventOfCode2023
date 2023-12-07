namespace AoC2023.Day07;

public class Day07 : IMDay
{
    public string FilePath { private get; init; } = "Day07\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput(false);
        return GetWinnings(input).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput(true);
        return GetWinnings(input).ToString();
    }

    private static int GetWinnings(Hand[] hands)
    {
        var sorted = hands.OrderBy(h => h.Strength).ToArray();

        return Enumerable.Range(0, sorted.Length)
            .Sum(i => (i + 1) * sorted[i].Bid);
    }

    private async Task<Hand[]> GetInput(bool useJokers) =>
        (await FileParser.ReadLinesAsStringArray(FilePath, " "))
        .Select(l => new Hand(l[0].ToCharArray(), l[1].ParseToInt(), useJokers))
        .ToArray();
}
