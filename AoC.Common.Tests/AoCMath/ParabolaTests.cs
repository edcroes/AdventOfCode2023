namespace AoC.Common.Tests.AoCMath;

[TestClass]
public class ParabolaTests
{
    [TestMethod]
    public void ShouldReturnTheCorrectXForYForSimpleOpensDownParabola()
    {
        var parabola = new Parabola(new(3, 9), new(0, 0));
        Assert.AreEqual(2, parabola.GetXForY(8));
        Assert.AreEqual(1, parabola.GetXForY(5));
        Assert.AreEqual(0, parabola.GetXForY(0));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectXForYForSimpleOpensLeftParabola()
    {
        var parabola = new Parabola(new(9, 3), new(0, 0), Parabola.ParabolaType.OpensHorizontally);
        Assert.AreEqual(0, parabola.GetXForY(0));
        Assert.AreEqual(5, parabola.GetXForY(1));
        Assert.AreEqual(8, parabola.GetXForY(2));
        Assert.AreEqual(9, parabola.GetXForY(3));
        Assert.AreEqual(8, parabola.GetXForY(4));
        Assert.AreEqual(5, parabola.GetXForY(5));
        Assert.AreEqual(0, parabola.GetXForY(6));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectYForXForSimpleOpensDownParabola()
    {
        var parabola = new Parabola(new(3, 9), new(0, 0));
        Assert.AreEqual(0, parabola.GetYForX(0));
        Assert.AreEqual(5, parabola.GetYForX(1));
        Assert.AreEqual(8, parabola.GetYForX(2));
        Assert.AreEqual(9, parabola.GetYForX(3));
        Assert.AreEqual(8, parabola.GetYForX(4));
        Assert.AreEqual(5, parabola.GetYForX(5));
        Assert.AreEqual(0, parabola.GetYForX(6));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectYForXForSimpleOpensLeftParabola()
    {
        var parabola = new Parabola(new(9, 3), new(0, 0), Parabola.ParabolaType.OpensHorizontally);
        Assert.AreEqual(2, parabola.GetYForX(8));
        Assert.AreEqual(1, parabola.GetYForX(5));
        Assert.AreEqual(0, parabola.GetYForX(0));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectXForYForSimpleOpensUpParabola()
    {
        var parabola = new Parabola(new(2.5, -6.25), new(0, 0));
        Assert.AreEqual(2, parabola.GetXForY(-6));
        Assert.AreEqual(1, parabola.GetXForY(-4));
        Assert.AreEqual(0, parabola.GetXForY(0));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectXForYForSimpleOpensRightParabola()
    {
        var parabola = new Parabola(new(-6.25, 2.5), new(0, 0), Parabola.ParabolaType.OpensHorizontally);
        Assert.AreEqual(0, parabola.GetXForY(0));
        Assert.AreEqual(-4, parabola.GetXForY(1));
        Assert.AreEqual(-6, parabola.GetXForY(2));
        Assert.AreEqual(-6, parabola.GetXForY(3));
        Assert.AreEqual(-4, parabola.GetXForY(4));
        Assert.AreEqual(0, parabola.GetXForY(5));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectYForXForSimpleOpensUpParabola()
    {
        var parabola = new Parabola(new(2.5, -6.25), new(0, 0));
        Assert.AreEqual(0, parabola.GetYForX(0));
        Assert.AreEqual(-4, parabola.GetYForX(1));
        Assert.AreEqual(-6, parabola.GetYForX(2));
        Assert.AreEqual(-6, parabola.GetYForX(3));
        Assert.AreEqual(-4, parabola.GetYForX(4));
        Assert.AreEqual(0, parabola.GetYForX(5));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectYForXForSimpleOpensRightParabola()
    {
        var parabola = new Parabola(new(-6.25, 2.5), new(0, 0), Parabola.ParabolaType.OpensHorizontally);
        Assert.AreEqual(2, parabola.GetYForX(-6));
        Assert.AreEqual(1, parabola.GetYForX(-4));
        Assert.AreEqual(0, parabola.GetYForX(0));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectXForYForOpensDownParabola()
    {
        var parabola = new Parabola(new(4, 7), new(0, -25));
        Assert.AreEqual(3, parabola.GetXForY(5));
        Assert.AreEqual(2, parabola.GetXForY(-1));
        Assert.AreEqual(1, parabola.GetXForY(-11));
        Assert.AreEqual(0, parabola.GetXForY(-25));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectXForYForOpensRightParabola()
    {
        var parabola = new Parabola(new(7, 4), new(-25, 0), Parabola.ParabolaType.OpensHorizontally);
        Assert.AreEqual(-25, parabola.GetXForY(0));
        Assert.AreEqual(-11, parabola.GetXForY(1));
        Assert.AreEqual(-1, parabola.GetXForY(2));
        Assert.AreEqual(5, parabola.GetXForY(3));
        Assert.AreEqual(7, parabola.GetXForY(4));
        Assert.AreEqual(5, parabola.GetXForY(5));
        Assert.AreEqual(-1, parabola.GetXForY(6));
        Assert.AreEqual(-11, parabola.GetXForY(7));
        Assert.AreEqual(-25, parabola.GetXForY(8));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectYForXForOpensUpParabola()
    {
        var parabola = new Parabola(new(4, -7), new(0, 25));
        Assert.AreEqual(25, parabola.GetYForX(0));
        Assert.AreEqual(11, parabola.GetYForX(1));
        Assert.AreEqual(1, parabola.GetYForX(2));
        Assert.AreEqual(-5, parabola.GetYForX(3));
        Assert.AreEqual(-7, parabola.GetYForX(4));
        Assert.AreEqual(-5, parabola.GetYForX(5));
        Assert.AreEqual(1, parabola.GetYForX(6));
        Assert.AreEqual(11, parabola.GetYForX(7));
        Assert.AreEqual(25, parabola.GetYForX(8));
    }

    [TestMethod]
    public void ShouldReturnTheCorrectYForXForOpensLeftParabola()
    {
        var parabola = new Parabola(new(-7, 4), new(25, 0), Parabola.ParabolaType.OpensHorizontally);
        Assert.AreEqual(3, parabola.GetYForX(-5));
        Assert.AreEqual(2, parabola.GetYForX(1));
        Assert.AreEqual(1, parabola.GetYForX(11));
        Assert.AreEqual(0, parabola.GetYForX(25));
    }
}
