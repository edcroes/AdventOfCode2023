namespace AoC2022.Tests;

[TestClass]
public abstract class DayTestsBase<T>(string expectedAnswerPart1, string expectedAnswerPart2) where T : IMDay, new()
{
    private readonly IMDay _dayToTest = new T();

    [TestMethod]
    public async Task Part1Test()
    {
        if (expectedAnswerPart1 != "TODO")
        {
            var answerPart1 = await _dayToTest.GetAnswerPart1();
            Assert.AreEqual(expectedAnswerPart1, answerPart1);
        }
    }

    [TestMethod]
    public async Task Part2Test()
    {
        if (expectedAnswerPart2 != "TODO")
        {
            var answerPart2 = await _dayToTest.GetAnswerPart2();
            Assert.AreEqual(expectedAnswerPart2, answerPart2);
        }
    }
}
