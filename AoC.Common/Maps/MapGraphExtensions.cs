using AoC.Common.Graphs;

namespace AoC.Common.Maps;
public static class MapGraphExtensions
{
    public static WeightedGraph<Point> ToWeightedGraph<T>(this Map<T> map, Point start, ICollection<T> validPathItems, GraphStrategy strategy = GraphStrategy.KeepShortestPath) where T : notnull
    {
        HashSet<Point> verticesToProcess = [start];
        HashSet<Point> pointsHit = [start];
        WeightedGraph<Point> graph = new();

        while (verticesToProcess.Count > 0)
        {
            var current = verticesToProcess.First();
            var neighbors = map.GetStraightNeighbors(current)
                .Where(n => validPathItems.Contains(map.GetValue(n)));

            foreach (var neighbor in neighbors)
            {
                if (pointsHit.Contains(neighbor))
                    continue;

                pointsHit.Add(neighbor);

                var previous = current;
                var nextNeighbor = current;
                var steps = 0;

                Point[] nextNeighbors = [neighbor];
                while (nextNeighbors.Length == 1)
                {
                    steps++;
                    previous = nextNeighbor;
                    nextNeighbor = nextNeighbors[0];
                    pointsHit.Add(nextNeighbor);

                    // Loop detected
                    if (nextNeighbor == current)
                        break;

                    nextNeighbors = map
                        .GetStraightNeighbors(nextNeighbor)
                        .Where(n => n != previous && validPathItems.Contains(map.GetValue(n)))
                        .ToArray();
                }

                var existingWeight = graph.GetEdgeWeight(current, nextNeighbor);
                if (existingWeight is not null && (strategy == GraphStrategy.KeepShortestPath && existingWeight > steps) || (strategy == GraphStrategy.KeepLongestPath && existingWeight < steps))
                {
                    graph.RemoveEdge(current, nextNeighbor);
                    graph.AddEdge(current, nextNeighbor, steps);
                }
                else if (existingWeight is null)
                {
                    graph.AddEdge(current, nextNeighbor, steps);
                }

                verticesToProcess.Add(nextNeighbor);
            }

            verticesToProcess.Remove(current);
        }

        return graph;
    }
}

public enum GraphStrategy
{
    KeepShortestPath,
    KeepLongestPath
}
