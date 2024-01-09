using AoC.Common.Collections;

namespace AoC.Common.Graphs;
public static class WeightedGraphStoerWagnerExtensions
{
    public static (int cutWeight, HashSet<T> left, HashSet<T> right) GetStoerWagnerMinimumEdgeCut<T>(this WeightedGraph<T> graph, int? minimumCutToFind = null) where T : notnull
    {
        if (graph.Edges.Count == 0)
            throw new ArgumentException("The graph contains no edges");

        if (graph.Edges.Values.Any(v => v < 0))
            throw new ArgumentException("The graph can only have positive weights");

        var workingGraph = graph.ToGraphWithHashSets();

        HashSet<T> currentBestPartition = [];
        var currentBestCut = int.MaxValue;
        var start = workingGraph.Vertices[0];

        while (workingGraph.Edges.Count > 0)
        {
            var (secondToLast, last, weight) = MaximumAdjacencySearch(workingGraph, start);
            if (weight < currentBestCut)
            {
                currentBestCut = weight;
                currentBestPartition.Clear();
                currentBestPartition.AddRange(last);
            }

            if (currentBestCut <= minimumCutToFind)
                break;

            workingGraph.MergeVertices(secondToLast, last);
        }

        var others = graph.Vertices.Except(currentBestPartition).ToHashSet();

        return (currentBestCut, currentBestPartition, others);
    }

    private static (HashSet<T> secondToLast, HashSet<T> last, int weight) MaximumAdjacencySearch<T>(WeightedCollectionGraph<HashSet<T>, T> graph, HashSet<T> start) where T : notnull
    {
        HashSet<T> secondToLast = start;
        HashSet<T> last = start;

        List<HashSet<T>> vertices = new(graph.Vertices);
        vertices.Remove(start);

        PriorityQueueWithRemove<HashSet<T>, int> queue = new();
        foreach (HashSet<T> vertex in vertices)
        {
            var weight = graph.GetNeighbors(start).Contains(vertex)
                ? -graph.GetEdgeWeight(start, vertex)!.Value
                : 0;

            queue.Enqueue(vertex, weight);
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            vertices.Remove(current);

            secondToLast = last;
            last = current;

            foreach (var neighbor in graph.GetNeighbors(current))
            {
                if (vertices.Contains(neighbor) && queue.Remove(neighbor, out var _, out var weight))
                {
                    queue.Enqueue(neighbor, weight - graph.GetEdgeWeight(current, neighbor)!.Value);
                }
            }
        }

        return (secondToLast, last, graph.GetNeighbors(last).Sum(n => graph.GetEdgeWeight(last, n) ?? 0));
    }

    private static WeightedCollectionGraph<HashSet<T>, T> ToGraphWithHashSets<T>(this WeightedGraph<T> graph) where T : notnull
    {
        WeightedCollectionGraph<HashSet<T>, T> newGraph = new();
        var vertices = graph.Vertices.ToDictionary(v => v, v => new HashSet<T>() { v });
        
        foreach (var (from, to) in graph.Edges.Keys)
        {
            newGraph.AddEdge(vertices[from], vertices[to], graph.GetEdgeWeight(from, to)!.Value);
        }

        return newGraph;
    }
}
