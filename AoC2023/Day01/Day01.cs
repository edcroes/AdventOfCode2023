namespace AoC2023.Day01;

public class Day01 : IMDay
{
    public string FilePath { private get; init; } = "Day01\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();

        return input
            .Select(i => i.Where(c => c.IsNumber()))
            .Sum(s => $"{s.First()}{s.Last()}".ParseToInt())
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();

        return input
            .Select(GetNumbers)
            .Select(i => i.Where(c => c.IsNumber()))
            .Sum(s => $"{s.First()}{s.Last()}".ParseToInt())
            .ToString();
    }

    private static string GetNumbers(string line)
    {
        Dictionary<string, char> numbers = new()
        {
            { "one",   '1' },
            { "two",   '2' },
            { "three", '3' },
            { "four",  '4' },
            { "five",  '5' },
            { "six",   '6' },
            { "seven", '7' },
            { "eight", '8' },
            { "nine",  '9' }
        };

        var result = string.Empty;
        for (var i = 0; i < line.Length; i++)
        {
            var number = numbers.Keys.SingleOrDefault(n => line[i..].StartsWith(n));
            if (number.IsNotNullOrEmpty())
            {
                result += numbers[number];
                i += number.Length - 2;
            }
            else if (line[i].IsNumber())
                result += line[i];
        }

        return result;
    }

    private async Task<string[]> GetInput() =>
        await FileParser.ReadLinesAsString(FilePath);
}
