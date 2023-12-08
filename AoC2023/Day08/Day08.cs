namespace AoC2023.Day08;

public class Day08 : IMDay
{
    public string FilePath { private get; init; } = "Day08\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (directions, network) = await GetMap();

        return GetSteps("AAA", node => node == "ZZZ", directions!, network!).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (directions, network) = await GetMap();

        return network.Keys
            .Where(k => k[^1] == 'A')
            .Select(k => GetSteps(k, node => node[^1] == 'Z', directions, network))
            .Aggregate(AoCMath.LeastCommonMultiple)
            .ToString();
    }

    private static long GetSteps(string start, Func<string, bool> isDestination, int[] directions, Dictionary<string, string[]> network)
    {
        var steps = 0;
        while (true)
        {
            var nextStep = directions[steps % directions.Length];
            steps++;
            start = network[start][nextStep];

            if (isDestination(start))
                break;
        }

        return steps;
    }

    private async Task<(int[] directions, Dictionary<string, string[]> network)> GetMap()
    {
        var (first, second) = await GetInput();

        var directions = first!
            .First()
            .Select(c => c == 'L' ? 0 : 1)
            .ToArray();

        var network = second!
            .Select(ParseNode)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return (directions, network);
    }

    private KeyValuePair<string, string[]> ParseNode(string line)
    {
        var (node, right) = line.ToStringArray("=");
        var destinations = right!.Trim('(', ')').ToStringArray(",");

        return new KeyValuePair<string, string[]>(node!, destinations);
    }

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadBlocksAsStringArray(FilePath);
}
