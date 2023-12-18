namespace AoC2023.Day18;

public class Day18 : IMDay
{
    public string FilePath { private get; init; } = "Day18\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetBorders();
        return GetArea(input).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetSwappedBorders();
        return GetArea(input).ToString();
    }

    private static double GetArea(Border[] borders)
    {
        Point lastPoint = new(0, 0);
        List<Point> vertices = [lastPoint];

        foreach (var line in borders)
        {
            lastPoint = lastPoint.Add(new(line.Direction.X * line.Steps, line.Direction.Y * line.Steps));
            vertices.Add(lastPoint);
        }
        vertices.Reverse();
        return AoCMath.GetPolygonAreaWithBorder(vertices);
    }

    private async Task<Border[]> GetBorders() =>
        (await GetInput()).Select(ParseBorder).ToArray();

    private async Task<Border[]> GetSwappedBorders() =>
        (await GetInput()).Select(ParseSwappedBorder).ToArray();

    private static Border ParseBorder(string[] input)
    {
        Point direction = input[0] switch
        {
            "U" => new(0, -1),
            "D" => new(0, 1),
            "L" => new(-1, 0),
            "R" => new(1, 0),
            _ => throw new NotSupportedException($"{input[0]} is not a direction")
        };

        return new(direction, input[1].ParseToInt(), ColorTranslator.FromHtml(input[2][1..^1]));
    }

    private static Border ParseSwappedBorder(string[] input)
    {
        var instruction = input.Last();
        Point direction = instruction[^2] switch
        {
            '0' => new(1, 0),
            '1' => new(0, 1),
            '2' => new(-1, 0),
            '3' => new(0, -1),
            _   => throw new NotSupportedException($"{instruction[^2]} is not a direction")
        };
        var steps = Convert.ToInt32($"0x{instruction[2..^2]}", 16);

        return new(direction, steps, default);
    }

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadLinesAsStringArray(FilePath, " ");

    private record struct Border(Point Direction, int Steps, Color Color);
}
