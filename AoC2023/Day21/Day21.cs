namespace AoC2023.Day21;

public class Day21 : IMDay
{
    public string FilePath { private get; init; } = "Day21\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        var steps = IsTestInput ? 6 : 64;

        for (var i = 0; i < steps; i++)
            WalkTheWalk(map);

        return map.Count('S').ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        ValidateForPart2(map);

        var totalStepsToTake = IsTestInput
            ? map.SizeX / 2 + map.SizeX * 4
            : 26501365;

        var stepsToSide = map.SizeX / 2;
        long mapsToAddToOneSide = (totalStepsToTake - stepsToSide) / map.SizeX;

        Console.WriteLine($"Total steps to take: {totalStepsToTake}");
        Console.WriteLine($"Steps to side: {stepsToSide}, Maps to add: {mapsToAddToOneSide}");
        //                                                                      ^
        //       X X         X                                                / E \
        // Even:  X    Odd: X X   Eventually the field looks something like: <E O E>
        //       X X         X                                                \ E /
        //                                                                      v
        var numberOfOddBlocks = Math.Pow(mapsToAddToOneSide - 1, 2);
        var numberOfEvenBlocks = Math.Pow(mapsToAddToOneSide, 2);


        var stepsForLargeBlock = map.SizeX - 1 + map.SizeX / 2;
        var stepsForSmallBlock = map.SizeX / 2 - 1;

        var (even, odd) = GetEvenAndOddCompletedMapSteps(map);
        var leftPoint = GetNumberOfSteps(map, map.SizeX - 1, new(map.SizeX - 1, map.SizeY / 2));
        var rightPoint = GetNumberOfSteps(map, map.SizeX - 1, new(0, map.SizeY / 2));
        var topPoint = GetNumberOfSteps(map, map.SizeY - 1, new(map.SizeX / 2, map.SizeY - 1));
        var bottomPoint = GetNumberOfSteps(map, map.SizeY - 1, new(map.SizeX / 2, 0));

        var topLeftLarge = GetNumberOfSteps(map, stepsForLargeBlock, new(map.SizeX - 1, map.SizeY - 1));
        var topLeftSmall = GetNumberOfSteps(map, stepsForSmallBlock, new(map.SizeX - 1, map.SizeY - 1));
        var topRightLarge = GetNumberOfSteps(map, stepsForLargeBlock, new(0, map.SizeY - 1));
        var topRightSmall = GetNumberOfSteps(map, stepsForSmallBlock, new(0, map.SizeY - 1));

        var bottomLeftLarge = GetNumberOfSteps(map, stepsForLargeBlock, new(map.SizeX - 1, 0));
        var bottomLeftSmall = GetNumberOfSteps(map, stepsForSmallBlock, new(map.SizeX - 1, 0));
        var bottomRightLarge = GetNumberOfSteps(map, stepsForLargeBlock, new(0, 0));
        var bottomRightSmall = GetNumberOfSteps(map, stepsForSmallBlock, new(0, 0));

        var totals = leftPoint + rightPoint + bottomPoint + topPoint +
            even * numberOfEvenBlocks + odd * numberOfOddBlocks +
            new[] { topLeftLarge, topRightLarge, bottomLeftLarge, bottomRightLarge }.Sum(i => i * (mapsToAddToOneSide - 1)) +
            new[] { topLeftSmall, topRightSmall, bottomLeftSmall, bottomRightSmall }.Sum(i => i * mapsToAddToOneSide);

        return totals.ToString();
    }

    private static int GetNumberOfSteps(Map<char> map, int steps, Point start)
    {
        var copy = map.Clone();
        copy.SetValue(map.SizeX / 2, map.SizeY / 2, '.');
        copy.SetValue(start, 'S');

        for (var i = 0; i < steps; i++)
            WalkTheWalk(copy);

        return copy.Count('S');
    }

    private static (int, int) GetEvenAndOddCompletedMapSteps(Map<char> map)
    {
        Map<char> copy = map.Clone();
        var previousOptions = 0;
        var options = 0;

        var total = copy.SizeX / 2 + copy.SizeY / 2;

        for (var i = 0; i < total; i++)
        {
            previousOptions = options;
            WalkTheWalk(copy);
            options = copy.Count('S');
        }

        return (options, previousOptions);
    }

    private static void WalkTheWalk(Map<char> map)
    {
        map.DistributeChaos(
                'S',
                (p, v) => v != '#',
                (m, p) => m.GetStraightNeighbors(p).Where(n => m.GetValue(n) == 'S').Count(),
                (alive, matches) => matches > 0 ? 'S' : '.');
    }

    private static void ValidateForPart2(Map<char> map)
    {
        if (map.SizeX != map.SizeY)
            throw new NotSupportedException("Map should be square");

        if (map.SizeX % 2 == 0)
            throw new NotSupportedException("Map size should be odd");

        if (map.SizeX / 2 % 2 == 0)
            throw new NotSupportedException("Left and right of middle should be odd");

        if (map.GetLine(0, map.SizeY / 2, map.SizeX - 1, map.SizeY / 2).Any(v => v == '#') ||
            map.GetLine(map.SizeX / 2, 0, map.SizeX / 2, map.SizeY - 1).Any(v => v == '#'))
            throw new NotSupportedException("There should be a clear line of sight from the middle to the edges");

        if (map.GetLine(0, 0, map.SizeX - 1, 0).Any(v => v != '.') ||
            map.GetLine(0, 0, 0, map.SizeY - 1).Any(v => v != '.') ||
            map.GetLine(map.SizeX - 1, 0, map.SizeX - 1, map.SizeY - 1).Any(v => v != '.') ||
            map.GetLine(0, map.SizeY - 1, map.SizeX - 1, map.SizeY - 1).Any(v => v != '.'))
            throw new NotSupportedException("All edges should be clean");
    }

    private bool IsTestInput => FilePath.Contains("test");

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
