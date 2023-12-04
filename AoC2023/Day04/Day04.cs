namespace AoC2023.Day04;

public class Day04 : IMDay
{
    public string FilePath { private get; init; } = "Day04\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var cards = await GetInput();
        return cards
            .Where(c => c.Numbers.Any(n => c.WinningNumbers.Contains(n)))
            .Sum(c => Math.Pow(2, c.Numbers.Count(n => c.WinningNumbers.Contains(n)) - 1))
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var cards = await GetInput();
        Dictionary<int, int> cardCount = new(cards.Select(c => new KeyValuePair<int, int>(c.Id, 1)));

        foreach (var card in cards)
        {
            var winCount = card.Numbers.Count(n => card.WinningNumbers.Contains(n));
            for (var i = 1; i <= winCount; i++)
            {
                var cardId = card.Id + i;
                if (cardCount.TryGetValue(cardId, out int value))
                    cardCount[cardId] = value + cardCount[card.Id];
                else
                    break;
            }
        }

        return cardCount.Values.Sum().ToString();
    }

    private static Card ParseCard(string input)
    {
        var (card, allNumbers) = input.Split(": ");
        var (winning, owning) = allNumbers!.Split("|");
        var winningNumbers = winning!
            .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(n => n.ParseToInt())
            .ToArray();
        var owningNumbers = owning!
            .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(n => n.ParseToInt())
            .ToArray();

        return new(card!.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].ParseToInt(), winningNumbers, owningNumbers);
    }

    private async Task<IEnumerable<Card>> GetInput() =>
        (await FileParser.ReadLinesAsString(FilePath)).Select(ParseCard);

    private record Card(int Id, int[] WinningNumbers, int[] Numbers);
}
