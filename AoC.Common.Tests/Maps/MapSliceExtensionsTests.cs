using System.Drawing;
using AoC.Common.Maps;

namespace AoC.Common.Tests.Maps;

[TestClass]
public class MapSliceExtensionsTests
{
    [TestMethod]
    public void SliceShouldFailOnOutOfBoundsRectangle()
    {
        var map = GetMap();
        Rectangle[] rectangles = [
            new(0, 0, 5, 2),
            new(0, 0, 2, 5),
            new(-1, 0, 2, 2),
            new(0, -1, 2, 2),
            new(-1, -1, 6, 6)
        ];

        foreach (var rectangle in rectangles)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => map.Slice(rectangle));
            Assert.AreEqual("Rectangle exceeds the map size (Parameter 'rectangle')", exception.Message);
        }
    }

    [TestMethod]
    public void SliceShouldReturnTheSlicedMap()
    {
        var map = GetMap();
        Rectangle rectangle = new(2, 2, 2, 2);

        var result = map.Slice(rectangle);

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.SizeX);
        Assert.AreEqual(2, result.SizeY);
        Assert.AreEqual(22, result.GetValue(0, 0));
        Assert.AreEqual(23, result.GetValue(1, 0));
        Assert.AreEqual(32, result.GetValue(0, 1));
        Assert.AreEqual(33, result.GetValue(1, 1));
    }

    private static Map<int> GetMap() =>
        new([
            [0, 1, 2, 3],
            [10, 11, 12, 13],
            [20, 21, 22, 23],
            [30, 31, 32, 33]
        ]);
}
