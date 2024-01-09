using AoC.Common.Graphs;

namespace AoC.Common.Tests.Graphs;

[TestClass]
public class WeightedGraphTests
{
    [TestMethod]
    public void ShouldCalculateMinCutUsingStoerWagnerCorrectUsingPaperExample()
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

        var (minCut, left, right) = sut.GetStoerWagnerMinimumEdgeCut();

        var with1 = left.Contains(1) ? left : right;
        var without1 = left.Contains(1) ? right : left;

        Assert.AreEqual(4, minCut);
        Assert.AreEqual(4, with1.Count);
        Assert.AreEqual(4, without1.Count);

        Assert.IsTrue(with1.Contains(1));
        Assert.IsTrue(with1.Contains(2));
        Assert.IsTrue(with1.Contains(5));
        Assert.IsTrue(with1.Contains(6));

        Assert.IsTrue(without1.Contains(3));
        Assert.IsTrue(without1.Contains(4));
        Assert.IsTrue(without1.Contains(7));
        Assert.IsTrue(without1.Contains(8));
    }

    [TestMethod]
    public void ShouldCalculateMinCutUsingStoerWagnerCorrectOnOneEdge()
    {
        WeightedGraph<int> sut = new();
        sut.AddEdge(1, 2, 1);
        sut.AddEdge(1, 3, 1);
        sut.AddEdge(1, 4, 1);
        sut.AddEdge(2, 3, 1);
        sut.AddEdge(3, 4, 1);
        sut.AddEdge(3, 5, 1);
        sut.AddEdge(5, 6, 1);
        sut.AddEdge(5, 7, 1);
        sut.AddEdge(5, 8, 1);
        sut.AddEdge(6, 7, 1);
        sut.AddEdge(6, 8, 1);
        sut.AddEdge(7, 8, 1);

        var (minCut, left, right) = sut.GetStoerWagnerMinimumEdgeCut();

        var with1 = left.Contains(1) ? left : right;
        var without1 = left.Contains(1) ? right : left;

        Assert.AreEqual(1, minCut);
        Assert.AreEqual(4, with1.Count);
        Assert.AreEqual(4, without1.Count);

        Assert.IsTrue(with1.Contains(1));
        Assert.IsTrue(with1.Contains(2));
        Assert.IsTrue(with1.Contains(3));
        Assert.IsTrue(with1.Contains(4));

        Assert.IsTrue(without1.Contains(5));
        Assert.IsTrue(without1.Contains(6));
        Assert.IsTrue(without1.Contains(7));
        Assert.IsTrue(without1.Contains(8));
    }
}
