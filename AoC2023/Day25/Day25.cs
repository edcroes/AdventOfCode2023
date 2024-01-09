namespace AoC2023.Day25;

public class Day25 : IMDay
{
    public string FilePath { private get; init; } = "Day25\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetGraph();

        var (_, left, right) = input.GetStoerWagnerMinimumEdgeCut(3);
        return (left.Count * right.Count).ToString();
    }

    public Task<string> GetAnswerPart2()
    {
        return Task.FromResult("AoC IS DONE :)");
    }

    private async Task<WeightedGraph<string>> GetGraph()
    {
        WeightedGraph<string> graph = new();

        var input = await GetInput();
        foreach (var (from, others) in input)
        {
            foreach (var to in others!.Split(" "))
            {
                graph.AddEdge(from!, to, 1);
            }
        }

        return graph;
    }

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadLinesAsStringArray(FilePath, ":");
}
