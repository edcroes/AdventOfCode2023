namespace AoC2023.Day07;

public class Hand(char[] cards, int bid, bool useJokers)
{
    public int Bid => bid;

    // Value < 1000000 and Type < 100
    public int Strength { get; } = GetType(cards, useJokers) * 1000000 + GetValue(cards, useJokers);

    private static int GetType(char[] cards, bool useJokers)
    {
        var groups = cards.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

        if (useJokers)
        {
            groups.Remove('J', out var jokers);
            groups.AddOrUpdate(groups.Keys.OrderBy(k => groups[k]).LastOrDefault('J'), jokers);
        }

        return (10 - groups.Keys.Count) * 10 + groups.Values.Max();
    }

    private static int GetValue(char[] cards, bool useJokers)
    {
        return Enumerable.Range(0, cards.Length).Sum(i =>
            (int)Math.Pow(15, cards.Length - i - 1) * cards[i] switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => useJokers ? 1 : 11,
                'T' => 10,
                _ => cards[i].ToNumber()
            }
        );
    }
}
