namespace AoC2023.Day15;

public class Day15 : IMDay
{
    public string FilePath { private get; init; } = "Day15\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        return input.Sum(i => GetHashCode(i)).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();
        var boxes = Enumerable.Range(0, 256).Select(i => new List<Lens>()).ToArray();

        foreach (var lns in input)
        {
            var lens = Lens.Parse(lns);
            var box = GetHashCode(lens.Label);

            var lensInBox = boxes[box].SingleOrDefault(l => l.Label == lens.Label);
            if (lens.FocalLength == -1 && lensInBox != default)
            {
                boxes[box].Remove(lensInBox);
            }
            else if (lens.FocalLength >= 0)
            {
                if (lensInBox != default)
                    boxes[box][boxes[box].IndexOf(lensInBox)] = lens;
                else
                    boxes[box].Add(lens);
            }
        }

        return Enumerable.Range(0, 256)
            .Sum(i => boxes[i].Sum(l => (i + 1) * (boxes[i].IndexOf(l) + 1) * l.FocalLength))
            .ToString();
    }

    private static int GetHashCode(string value) =>
        value.Aggregate(0, (result, input) => (result + input) * 17 % 256);

    private async Task<string[]> GetInput() =>
        (await FileParser.ReadLinesAsStringArray(FilePath, ",")).Single();

    private record struct Lens(string Label, int FocalLength)
    {
        public static Lens Parse(string value)
        {
            if (value[^1] == '-')
                return new(value[0..^1], -1);

            var (label, length) = value.Split('=');
            return new(label!, length!.ParseToInt());
        }
    };
}
