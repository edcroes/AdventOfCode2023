namespace AoC.Common.Graphs;

public class WeightedGraph<T> where T : notnull
{
    private readonly Dictionary<T, HashSet<T>> _neighbors = [];
    private readonly Dictionary<(T from, T to), int> _edges = [];

    public IReadOnlyList<T> Vertices => _neighbors.Keys.ToList();

    public IReadOnlyDictionary<(T from, T to), int> Edges => _edges;

    public void AddEdge(T from, T to, int weigth)
    {
        ArgumentNullException.ThrowIfNull(from, nameof(from));
        ArgumentNullException.ThrowIfNull(to, nameof(to));

        var (first, second) = GetPair(from, to);

        if (!_edges.ContainsKey((first, second)))
        {
            _edges.Add((first, second), weigth);
            _neighbors.AddOrUpdate(from, to);
            _neighbors.AddOrUpdate(to, from);
        }
    }

    public void RemoveEdge(T from, T to)
    {
        ArgumentNullException.ThrowIfNull(from, nameof(from));
        ArgumentNullException.ThrowIfNull(to, nameof(to));

        var (first, second) = GetPair(from, to);
        if (_edges.Remove((first, second)))
        {
            _neighbors[from].Remove(to);
            _neighbors[to].Remove(from);

            if (_neighbors[from].Count == 0)
                _neighbors.Remove(from);

            if (_neighbors[to].Count == 0)
                _neighbors.Remove(to);
        }
    }

    public int? GetEdgeWeight(T from, T to) =>
        TryGetValue(_edges, from, to, out var result) ? result : null;

    public IReadOnlySet<T> GetNeighbors(T vertex) =>
        _neighbors.TryGetValue(vertex, out var neighbors)
            ? neighbors
            : [];

    public void MergeVertices(T first, T second)
    {
        RemoveEdge(first, second);
        var neighbors = GetNeighbors(first)
            .Union(GetNeighbors(second))
            .Distinct();

        var mergedVertex = GetMergedVertex(first, second);

        foreach (var neighbor in neighbors)
        {
            var weight = (GetEdgeWeight(first, neighbor) ?? 0) + (GetEdgeWeight(second, neighbor) ?? 0);
            RemoveEdge(first, neighbor);
            RemoveEdge(second, neighbor);
            AddEdge(mergedVertex, neighbor, weight);
        }
    }

    protected virtual T GetMergedVertex(T first, T second) => first;

    private static (T, T) GetPair(T from, T to)
    {
        var first = from.GetHashCode() < to.GetHashCode() ? from : to;
        var second = first.Equals(from) ? to : from;

        return (first, second);
    }

    private static bool TryGetValue(Dictionary<(T, T), int> graph, T from, T to, out int result)
    {
        var (first, second) = GetPair(from, to);

        if (graph.TryGetValue((first, second), out var weight))
        {
            result = weight;
            return true;
        }

        result = 0;
        return false;
    }
}
