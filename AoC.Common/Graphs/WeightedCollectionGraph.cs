namespace AoC.Common.Graphs;

public class WeightedCollectionGraph<TCollection, TItem> : WeightedGraph<TCollection> where TCollection : ICollection<TItem>, new()
{
    protected override TCollection GetMergedVertex(TCollection first, TCollection second)
    {
        TCollection newVertex = new();
        newVertex.AddRange(first);
        newVertex.AddRange(second);

        return newVertex;
    }
}
