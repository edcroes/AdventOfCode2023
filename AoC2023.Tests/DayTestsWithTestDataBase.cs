namespace AoC2023.Tests;

public class DayTestsWithTestDataBase<T>(
        string expectedAnswerPart1,
        string expectedAnswerPart2,
        string expectedAnswerPart1TestData,
        string expectedAnswerPart2TestData) : DayTestsBase<T>(expectedAnswerPart1, expectedAnswerPart2) where T : IMDay, new()
{
    private readonly IMDay _dayWithTestDataToTest = new T() { FilePath = $"TestData\\{typeof(T).Name}-testinput.txt" };

    [TestMethod]
    public async Task Part1WithTestDataTest()
    {
        var answerPart1 = await _dayWithTestDataToTest.GetAnswerPart1();
        Assert.AreEqual(expectedAnswerPart1TestData, answerPart1);
    }

    [TestMethod]
    public async Task Part2WithTestDataTest()
    {
        var answerPart2 = await _dayWithTestDataToTest.GetAnswerPart2();
        Assert.AreEqual(expectedAnswerPart2TestData, answerPart2);
    }
}
