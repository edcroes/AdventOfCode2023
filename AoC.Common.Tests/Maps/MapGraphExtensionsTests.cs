using AoC.Common.Maps;

namespace AoC.Common.Tests.Maps;

[TestClass]
public class MapGraphExtensionsTests
{
    [TestMethod]
    public void ToWeightedGraphShouldReturnTheCorrectGraphWithDeadEnd()
    {
        Map<char> sut = new([
            "#.###".ToCharArray(),
            "#...#".ToCharArray(),
            "###.#".ToCharArray(),
            "#####".ToCharArray()]);

        var graph = sut.ToWeightedGraph(new(1, 0), ['.']);

        Assert.AreEqual(2, graph.Vertices.Count);
        Assert.AreEqual(1, graph.Edges.Count);

        Assert.IsTrue(graph.Vertices.Contains(new(1, 0)));
        Assert.IsTrue(graph.Vertices.Contains(new(3, 2)));

        Assert.AreEqual(4, graph.GetEdgeWeight(new(1, 0), new(3, 2)));
    }

    [TestMethod]
    public void ToWeightedGraphShouldReturnTheCorrectGraphWithDeadEndAndExit()
    {
        Map<char> sut = new([
            "#...#".ToCharArray(),
            "#.#.#".ToCharArray(),
            "#.#..".ToCharArray(),
            "#####".ToCharArray()]);

        var graph = sut.ToWeightedGraph(new(1, 0), ['.']);

        Assert.AreEqual(3, graph.Vertices.Count);
        Assert.AreEqual(2, graph.Edges.Count);

        Assert.IsTrue(graph.Vertices.Contains(new(1, 0)));
        Assert.IsTrue(graph.Vertices.Contains(new(1, 2)));
        Assert.IsTrue(graph.Vertices.Contains(new(4, 2)));

        Assert.AreEqual(2, graph.GetEdgeWeight(new(1, 0), new(1, 2)));
        Assert.AreEqual(5, graph.GetEdgeWeight(new(1, 0), new(4, 2)));
        Assert.IsNull(graph.GetEdgeWeight(new(1, 2), new(4, 2)));
    }

    [TestMethod]
    public void ToWeightedGraphShouldReturnTheCorrectGraphWithLoop()
    {
        Map<char> sut = new([
            "#####".ToCharArray(),
            "#...#".ToCharArray(),
            "#.#.#".ToCharArray(),
            "#...#".ToCharArray(),
            "#####".ToCharArray()]);

        var graph = sut.ToWeightedGraph(new(1, 1), ['.']);

        Assert.AreEqual(1, graph.Vertices.Count);
        Assert.AreEqual(1, graph.Edges.Count);

        Assert.IsTrue(graph.Vertices.Contains(new(1, 1)));

        Assert.AreEqual(8, graph.GetEdgeWeight(new(1, 1), new(1, 1)));
    }

    [TestMethod]
    public void ToWeightedGraphShouldReturnTheCorrectGraphWithLoopAndExit()
    {
        Map<char> sut = new([
            "#.###".ToCharArray(),
            "#....".ToCharArray(),
            "##.##".ToCharArray(),
            "#...#".ToCharArray(),
            "#.#.#".ToCharArray(),
            "#...#".ToCharArray(),
            "#####".ToCharArray()]);

        var graph = sut.ToWeightedGraph(new(1, 0), ['.']);

        Assert.AreEqual(4, graph.Vertices.Count);
        Assert.AreEqual(4, graph.Edges.Count);

        Assert.IsTrue(graph.Vertices.Contains(new(1, 0)));
        Assert.IsTrue(graph.Vertices.Contains(new(2, 1)));
        Assert.IsTrue(graph.Vertices.Contains(new(4, 1)));
        Assert.IsTrue(graph.Vertices.Contains(new(2, 3)));

        Assert.AreEqual(2, graph.GetEdgeWeight(new(1, 0), new(2, 1)));
        Assert.AreEqual(2, graph.GetEdgeWeight(new(2, 1), new(4, 1)));
        Assert.AreEqual(2, graph.GetEdgeWeight(new(2, 1), new(2, 3)));
        Assert.AreEqual(8, graph.GetEdgeWeight(new(2, 3), new(2, 3)));
    }

    [TestMethod]
    public void ToWeightedGraphShouldReturnTheCorrectGraphWithShortestMultiPathAndExit()
    {
        Map<char> sut = new([
            "#####".ToCharArray(),
            "#...#".ToCharArray(),
            "#.#..".ToCharArray(),
            "#...#".ToCharArray(),
            "#####".ToCharArray()]);

        var graph = sut.ToWeightedGraph(new(1, 1), ['.']);

        Assert.AreEqual(3, graph.Vertices.Count);
        Assert.AreEqual(2, graph.Edges.Count);

        Assert.IsTrue(graph.Vertices.Contains(new(1, 1)));
        Assert.IsTrue(graph.Vertices.Contains(new(3, 2)));
        Assert.IsTrue(graph.Vertices.Contains(new(4, 2)));

        Assert.AreEqual(3, graph.GetEdgeWeight(new(1, 1), new(3, 2)));
        Assert.AreEqual(1, graph.GetEdgeWeight(new(3, 2), new(4, 2)));
    }

    [TestMethod]
    public void ToWeightedGraphShouldReturnTheCorrectGraphWithLongestMultiPathAndExit()
    {
        Map<char> sut = new([
            "#####".ToCharArray(),
            "#...#".ToCharArray(),
            "#.#..".ToCharArray(),
            "#...#".ToCharArray(),
            "#####".ToCharArray()]);

        var graph = sut.ToWeightedGraph(new(1, 1), ['.'], GraphStrategy.KeepLongestPath);

        Assert.AreEqual(3, graph.Vertices.Count);
        Assert.AreEqual(2, graph.Edges.Count);

        Assert.IsTrue(graph.Vertices.Contains(new(1, 1)));
        Assert.IsTrue(graph.Vertices.Contains(new(3, 2)));
        Assert.IsTrue(graph.Vertices.Contains(new(4, 2)));

        Assert.AreEqual(5, graph.GetEdgeWeight(new(1, 1), new(3, 2)));
        Assert.AreEqual(1, graph.GetEdgeWeight(new(3, 2), new(4, 2)));
    }

    [TestMethod]
    public void ToWeightedGraphShouldReturnTheCorrectGraph()
    {
        Map<char> sut = new([
            "#.#####".ToCharArray(),
            "#...###".ToCharArray(),
            "#.#....".ToCharArray(),
            "#.#.#.#".ToCharArray(),
            "#.....#".ToCharArray(),
            "#######".ToCharArray()]);

        var graph = sut.ToWeightedGraph(new(1, 1), ['.']);
        
        Assert.AreEqual(6, graph.Vertices.Count);
        Assert.AreEqual(7, graph.Edges.Count);

        Assert.IsTrue(graph.Vertices.Contains(new(1, 0)));
        Assert.IsTrue(graph.Vertices.Contains(new(1, 1)));
        Assert.IsTrue(graph.Vertices.Contains(new(3, 2)));
        Assert.IsTrue(graph.Vertices.Contains(new(3, 4)));
        Assert.IsTrue(graph.Vertices.Contains(new(5, 2)));
        Assert.IsTrue(graph.Vertices.Contains(new(6, 2)));

        Assert.AreEqual(1, graph.GetEdgeWeight(new(1, 0), new(1, 1)));
        Assert.AreEqual(3, graph.GetEdgeWeight(new(1, 1), new(3, 2)));
        Assert.AreEqual(5, graph.GetEdgeWeight(new(1, 1), new(3, 4)));
        Assert.AreEqual(2, graph.GetEdgeWeight(new(3, 2), new(3, 4)));
        Assert.AreEqual(2, graph.GetEdgeWeight(new(3, 2), new(5, 2)));
        Assert.AreEqual(4, graph.GetEdgeWeight(new(3, 4), new(5, 2)));
        Assert.AreEqual(1, graph.GetEdgeWeight(new(5, 2), new(6, 2)));
    }
}
