namespace AoC2023.Day16;

public class Day16 : IMDay
{
    public string FilePath { private get; init; } = "Day16\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        return GetEnergizedTiles(map, new(-1, 0), new(1, 0)).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        var borderPoints = map.Where((p, v) => p.X == 0 || p.X == map.SizeX - 1 || p.Y == 0 || p.Y == map.SizeY - 1);
        var max = 0;

        foreach (var start in borderPoints)
        {
            if (start.X == 0)
                max = Math.Max(max, GetEnergizedTiles(map, new(-1, start.Y), new(1, 0)));

            if (start.Y == 0)
                max = Math.Max(max, GetEnergizedTiles(map, new(start.X, -1), new(0, 1)));

            if (start.X == map.SizeX - 1)
                max = Math.Max(max, GetEnergizedTiles(map, new(map.SizeX, start.Y), new(-1, 0)));

            if (start.Y == map.SizeY - 1)
                max = Math.Max(max, GetEnergizedTiles(map, new(start.X, map.SizeY), new(0, -1)));
        }

        return max.ToString();
    }

    private static int GetEnergizedTiles(Map<char> map, Point start, Point startDirection)
    {
        Dictionary<Point, List<Point>> hitPoints = [];
        List<(Point position, Point direction)> beams = [(start, startDirection)];

        while (beams.Count > 0)
        {
            beams = GetNextLocations(map, beams);
            foreach (var (position, direction) in beams.ToArray())
            {
                if (hitPoints.TryGetValue(position, out var value) && value.Contains(direction))
                {
                    beams.Remove((position, direction));
                }
                else
                {
                    hitPoints.AddOrUpdate(position, [direction]);
                }
            }
        }

        return hitPoints.Keys.Count;
    }

    private static List<(Point, Point)> GetNextLocations(Map<char> map, List<(Point position, Point direction)> locations)
    {
        List<(Point, Point)> newLocations = [];
        foreach (var (position, direction) in locations)
        {
            var next = position.Add(direction);
            if (!map.Contains(next))
                continue;

            List<Point> nextDirections = map.GetValue(next) switch
            {
                '/' => [new(direction.Y * -1, direction.X * -1)],
                '\\'  => [new(direction.Y, direction.X)],
                '-' when direction.Y != 0 => [new(-1, 0), new(1, 0)],
                '|' when direction.X != 0 => [new(0, -1), new(0, 1)],
                _    => [direction]
            };

            foreach (var newDirection in nextDirections)
                newLocations.Add((next, newDirection));
        }

        return newLocations;
    }

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
