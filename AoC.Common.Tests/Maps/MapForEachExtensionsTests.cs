using System.Drawing;
using AoC.Common.Maps;

namespace AoC.Common.Tests.Maps;

[TestClass]
public class MapForEachExtensionsTests
{
    [TestMethod]
    public void ForEachInTriangleShouldHitTheCorrectPoints()
    {
        Dictionary<Point, int> pointsHit = [];
        var map = GetMap();

        map.ForEachInTriangle(new(3, 0), new(3, 2), new(1, 2), pointsHit.Add);

        Assert.AreEqual(6, pointsHit.Count);
        Assert.IsTrue(pointsHit.ContainsKey(new(3, 0)));
        Assert.IsTrue(pointsHit.ContainsKey(new(3, 1)));
        Assert.IsTrue(pointsHit.ContainsKey(new(3, 2)));
        Assert.IsTrue(pointsHit.ContainsKey(new(2, 1)));
        Assert.IsTrue(pointsHit.ContainsKey(new(1, 2)));
        Assert.IsTrue(pointsHit.ContainsKey(new(2, 2)));
        Assert.AreEqual(3, pointsHit[new(3, 0)]);
        Assert.AreEqual(13, pointsHit[new(3, 1)]);
        Assert.AreEqual(23, pointsHit[new(3, 2)]);
        Assert.AreEqual(12, pointsHit[new(2, 1)]);
        Assert.AreEqual(21, pointsHit[new(1, 2)]);
        Assert.AreEqual(22, pointsHit[new(2, 2)]);
    }

    [TestMethod]
    public void ForEachInRectangleShouldHitTheCorrectPoints()
    {
        Dictionary<Point, int> pointsHit = [];
        var map = GetMap();

        map.ForEachInRectangle(new(2, 1, 2, 2), pointsHit.Add);

        Assert.AreEqual(4, pointsHit.Count);
        Assert.IsTrue(pointsHit.ContainsKey(new(2, 1)));
        Assert.IsTrue(pointsHit.ContainsKey(new(3, 1)));
        Assert.IsTrue(pointsHit.ContainsKey(new(2, 2)));
        Assert.IsTrue(pointsHit.ContainsKey(new(3, 2)));
        Assert.AreEqual(12, pointsHit[new(2, 1)]);
        Assert.AreEqual(13, pointsHit[new(3, 1)]);
        Assert.AreEqual(22, pointsHit[new(2, 2)]);
        Assert.AreEqual(23, pointsHit[new(3, 2)]);
    }

    private static Map<int> GetMap() =>
        new([
            [ 0,  1,  2,  3],
            [10, 11, 12, 13],
            [20, 21, 22, 23],
            [30, 31, 32, 33]
        ]);
}
