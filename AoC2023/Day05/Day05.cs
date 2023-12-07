namespace AoC2023.Day05;

public class Day05 : IMDay
{
    public string FilePath { private get; init; } = "Day05\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (seeds, maps) = await GetParsedInput();
        return seeds.Min(s => GetDestination(s, maps)).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (seeds, maps) = await GetParsedInput();
        var seedRanges = Enumerable.Range(0, seeds.Length / 2)
            .Select(i => new AoCRange(seeds[i * 2], seeds[i * 2 + 1]))
            .ToArray();

        maps = maps
            .Select(m => m
                .Union(GetMissingRules(m))
                .OrderBy(r => r.Destination.Start)
                .ToArray())
            .ToArray();

        var destination = long.MaxValue;
        foreach (var rule in maps.Last())
        {
            var possibleSeeds = GetPossibleSeedsBottomUp(rule, maps.SkipLast(1).ToArray(), seedRanges);
            if (possibleSeeds.Count > 0)
            {
                destination = possibleSeeds.Min(s => GetDestination(s, maps));
                break;
            }
        }

        return destination.ToString();
    }

    private static List<long> GetPossibleSeedsBottomUp(AlmanacMapRule rule, AlmanacMapRule[][] upperRules, AoCRange[] seedRanges)
    {
        if (upperRules.Length == 0)
        {
            return seedRanges
                .Where(s => s.HasOverlap(rule.Source))
                .Select(s => s.Intersect(rule.Source).Start)
                .ToList();
        }

        List<long> possibleSeeds = [];

        foreach (var upperRule in upperRules[^1].Where(r => r.Destination.HasOverlap(rule.Source)))
        {
            var newDestinationRange = upperRule.Destination.Intersect(rule.Source);
            AoCRange newSourceRange = new(newDestinationRange.Start - upperRule.SourceToDestinationDifference, newDestinationRange.Length);
            AlmanacMapRule newUpperRule = new(newDestinationRange, newSourceRange);

            var seeds = GetPossibleSeedsBottomUp(newUpperRule, upperRules.SkipLast(1).ToArray(), seedRanges);
            possibleSeeds.AddRange(seeds);
        }

        return possibleSeeds;
    }

    private static List<AlmanacMapRule> GetMissingRules(AlmanacMapRule[] rules)
    {
        var ordered = rules.OrderBy(r => r.Source.Start).ToArray();
        var nextStart = 0L;
        List<AlmanacMapRule> newRules = [];

        for (var i = 0; i < ordered.Length; i++)
        {
            if (nextStart < ordered[i].Source.Start)
            {
                AoCRange range = new(nextStart, ordered[i].Source.Start - nextStart);
                newRules.Add(new(range, range));
            }

            nextStart = ordered[i].Source.End + 1;
        }

        AoCRange lastRange = new(ordered[^1].Source.End + 1, long.MaxValue - ordered[^1].Source.End);
        newRules.Add(new(lastRange, lastRange));

        return newRules;
    }

    private static long GetDestination(long seed, AlmanacMapRule[][] maps)
    {
        var source = seed;
        for (var i = 0; i < maps.Length; i++)
        {
            var rule = maps[i].SingleOrDefault(r => r.ContainsSource(source));
            if (rule != default)
            {
                source = rule.GetDestination(source);
            }
        }

        return source;
    }

    private async Task<(long[], AlmanacMapRule[][])> GetParsedInput()
    {
        var input = await GetInput();
        var seeds = input.First()[0][7..].ToLongArray(" ");
        return (seeds, input.Skip(1).Select(ParseMap).ToArray());
    }

    private static AlmanacMapRule[] ParseMap(string[] block) =>
        block.Skip(1).Select(ParseMapRule).ToArray();

    private static AlmanacMapRule ParseMapRule(string line)
    {
        var (destination, source, range) = line.ToLongArray(" ");
        return new(new(destination, range), new(source, range));
    }

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadBlocksAsStringArray(FilePath);

    private record struct AlmanacMapRule(AoCRange Destination, AoCRange Source)
    {
        public readonly long SourceToDestinationDifference =>
            Destination.Start - Source.Start;

        public readonly bool ContainsSource(long source) =>
            Source.Contains(source);

        public readonly long GetDestination(long source) =>
            source - Source.Start + Destination.Start;
    }
}
