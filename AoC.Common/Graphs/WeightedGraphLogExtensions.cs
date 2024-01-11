using System.Text;

namespace AoC.Common.Graphs;
public static class WeightedGraphLogExtensions
{
    public static void DumpWeightedGraphAsGraphVizToConsole<T>(this WeightedGraph<T> graph) where T : notnull =>
        Console.WriteLine(graph.DumpWeightedMapAsGraphVizToString());

    public static void DumpWeightedGraphAsGraphVizToConsole<T>(this WeightedGraph<T> graph, Func<T, string?> toString) where T : notnull =>
        Console.WriteLine(graph.DumpWeightedMapAsGraphVizToString(toString));

    public static async Task  DumpWeightedGraphAsGraphVizToFile<T>(this WeightedGraph<T> graph, string filePath) where T : notnull =>
        await File.WriteAllTextAsync(filePath, graph.DumpWeightedMapAsGraphVizToString());

    public static async Task DumpWeightedGraphAsGraphVizToFile<T>(this WeightedGraph<T> graph, string filePath, Func<T, string?> toString) where T : notnull =>
        await File.WriteAllTextAsync(filePath, graph.DumpWeightedMapAsGraphVizToString(toString));

    public static string DumpWeightedMapAsGraphVizToString<T>(this WeightedGraph<T> graph) where T : notnull =>
        graph.DumpWeightedMapAsGraphVizToString(t => t.ToString());

    public static string DumpWeightedMapAsGraphVizToString<T>(this WeightedGraph<T> graph, Func<T, string?> toString) where T : notnull
    {
        StringBuilder builder = new();
        builder.AppendLine("strict graph map {");
        HashSet<T> visited = [];

        foreach (var vertex in graph.Vertices)
        {
            var neighbors = graph.GetNeighbors(vertex).Where(p => !visited.Contains(p));

            foreach (var neighbor in neighbors)
                builder.AppendLine($"    {toString(vertex)} -- {toString(neighbor)} [label={graph.GetEdgeWeight(vertex, neighbor)}]");

            visited.Add(vertex);
        }

        builder.AppendLine("}");

        return builder.ToString();
    }
}
