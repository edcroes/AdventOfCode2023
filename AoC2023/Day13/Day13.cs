namespace AoC2023.Day13;

public class Day13 : IMDay
{
    public string FilePath { private get; init; } = "Day13\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var maps = await GetMaps();
        return maps.Sum(GetReflectionValue).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var maps = await GetMaps();
        return maps.Sum(GetSmudgedReflectionValue).ToString();
    }

    private static int GetSmudgedReflectionValue(Map<char> map)
    {
        var initialX = map.GetMirrorOnX();
        var initialY = map.GetMirrorOnY();

        var value = map.GetMirrorOnX(initialX, 1);
        
        if (value >= 0)
            return value + 1;

        value = map.GetMirrorOnY(initialY, 1);
        return (value + 1) * 100;
    }

    private static int GetReflectionValue(Map<char> map)
    {
        var value = map.GetMirrorOnX();
        if (value >= 0)
        {
            return value + 1;
        }

        value = map.GetMirrorOnY();
        return (value + 1) * 100;
    }

    private async Task<Map<char>[]> GetMaps() =>
        (await GetInput())
            .Select(i => new Map<char>(i.Select(m => m.ToCharArray()).ToArray()))
            .ToArray();

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadBlocksAsStringArray(FilePath);
}
