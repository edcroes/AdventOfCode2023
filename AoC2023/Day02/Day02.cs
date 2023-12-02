namespace AoC2023.Day02;

public class Day02 : IMDay
{
    private const int MaxRed = 12;
    private const int MaxGreen = 13;
    private const int MaxBlue = 14;

    public string FilePath { private get; init; } = "Day02\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        return input
            .Select(ParseGame)
            .Where(g => g.Sets.All(s => s.Red <= MaxRed && s.Green <= MaxGreen && s.Blue <= MaxBlue))
            .Sum(g => g.Id)
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();
        return input
            .Select(ParseGame)
            .Sum(g => g.Sets.Max(s => s.Red) * g.Sets.Max(s => s.Green) * g.Sets.Max(s => s.Blue))
            .ToString();
    }

    private static Game ParseGame(string[] input) =>
        new(input[0].Split(" ")[1].ParseToInt(), input[1].Split(';').Select(ParseSet).ToList());

    private static Set ParseSet(string input)
    {
        Dictionary<string, int> colors = new()
        {
            { "red", 0},
            { "green", 0 },
            { "blue", 0 }
        };

        foreach (var cubes in input.Split(",", StringSplitOptions.TrimEntries))
        {
            var (number, color) = cubes.Split(" ");
            colors[color!] = number!.ParseToInt();
        }

        return new(colors["red"], colors["green"], colors["blue"]);
    }

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadLinesAsStringArray(FilePath, ":");

    private record Game(int Id, List<Set> Sets);
    private record struct Set(int Red = 0, int Green = 0, int Blue = 0);
}
