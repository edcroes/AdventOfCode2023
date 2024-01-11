namespace AoC.Common.Graphs;

public static class WeightedGraphPathExtensions
{
    public static int GetLongestPath<T>(this WeightedGraph<T> graph, T from, T to) where T : notnull =>
        graph.GetLongestPath([from], to);

    private static int GetLongestPath<T>(this WeightedGraph<T> graph, List<T> route, T to) where T : notnull
    {
        var cost = 0;
        var neighbors = graph.GetNeighbors(route[^1]).Where(n => !route.Contains(n));

        foreach (var neighbor in neighbors)
        {
            var stepCost = graph.GetEdgeWeight(route[^1], neighbor)!.Value;
            if (neighbor.Equals(to))
            {
                cost = Math.Max(cost, stepCost);
            }
            else
            {
                var routeCost = graph.GetLongestPath([.. route, neighbor], to);

                if (routeCost != 0)
                    cost = Math.Max(cost, stepCost + routeCost);
            }
        }

        return cost;
    }
}
