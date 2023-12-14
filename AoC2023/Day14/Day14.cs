namespace AoC2023.Day14;

public class Day14 : IMDay
{
    public string FilePath { private get; init; } = "Day14\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        Tilt(map, Direction.North);
        var rocks = map.Where((p, v) => v == 'O');
        return rocks.Sum(r => map.SizeY - r.Y).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        
        Dictionary<int, int> cycleHashes = [];
        Dictionary<int, Point[]> cycleRocks = [];
        const int cycles = 1000000000;
        Point[] rocks = [];

        var cycle = 0;

        while(true)
        {
            Tilt(map, Direction.North);
            Tilt(map, Direction.West);
            Tilt(map, Direction.South);
            Tilt(map, Direction.East);

            var currentRocks = map.Where((p, v) => v == 'O').ToArray();
            var hash = currentRocks.GetAoCHashCode();

            if (cycleHashes.TryGetValue(hash, out int otherCycle))
            {
                var recurringCycleTime = cycle - otherCycle;
                var endCycle = (cycles - otherCycle) % recurringCycleTime + otherCycle - 1;
                if (endCycle < otherCycle)
                    endCycle += recurringCycleTime;

                rocks = cycleRocks[endCycle];
                break;
            }
            else
            {
                cycleHashes.Add(hash, cycle);
                cycleRocks.Add(cycle, currentRocks);
            }

            cycle++;
        }

        return rocks.Sum(r => map.SizeY - r.Y).ToString();
    }

    private static void Tilt(Map<char> map, Direction direction)
    {
        var order = direction.IsIn(Direction.North, Direction.West)
            ? ForEachOrder.LeftRightTopBottom
            : ForEachOrder.BottomTopRightLeft;
        var move = direction.IsIn(Direction.South, Direction.East)
            ? 1 :
            -1;

        map.ForEach(order, (p, v) =>
        {
            if (v != 'O')
                return;

            var newIndex = move + (direction.IsIn(Direction.North, Direction.South) ? p.Y : p.X);
            var size = direction.IsIn(Direction.North, Direction.South) ? map.SizeY : map.SizeX;

            while (newIndex >= 0 && newIndex < size)
            {
                var value = direction.IsIn(Direction.North, Direction.South)
                    ? map.GetValue(p.X, newIndex)
                    : map.GetValue(newIndex, p.Y);
                if (value != '.')
                    break;
                
                newIndex += move;
            }

            newIndex -= move;
            if (newIndex != (direction.IsIn(Direction.North, Direction.South) ? p.Y : p.X))
            {
                Point next = direction.IsIn(Direction.North, Direction.South)
                    ? new(p.X, newIndex)
                    : new(newIndex, p.Y);
                map.SetValue(next, 'O');
                map.SetValue(p, '.');
            }
        });
    }

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));

    private enum Direction
    {
        North,
        West,
        South,
        East
    }
}
