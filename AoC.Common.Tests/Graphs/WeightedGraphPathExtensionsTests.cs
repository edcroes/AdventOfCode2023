using AoC.Common.Graphs;

namespace AoC.Common.Tests.Graphs;

[TestClass]
public class WeightedGraphPathExtensionsTests
{
    [TestMethod]
    public void LongestPathShouldBeCorrectForStoerWagnerExample()
    {
        WeightedGraph<int> sut = new();
        sut.AddEdge(1, 2, 2);
        sut.AddEdge(1, 5, 3);
        sut.AddEdge(2, 3, 3);
        sut.AddEdge(2, 5, 2);
        sut.AddEdge(2, 6, 2);
        sut.AddEdge(3, 4, 4);
        sut.AddEdge(3, 7, 2);
        sut.AddEdge(4, 7, 2);
        sut.AddEdge(4, 8, 2);
        sut.AddEdge(5, 6, 3);
        sut.AddEdge(6, 7, 1);
        sut.AddEdge(7, 8, 3);

        var result = sut.GetLongestPath(1, 8);

        Assert.AreEqual(20, result);
    }
}
