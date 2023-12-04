using AdventOfCode2023;

namespace Day04;

internal class Day04 : BaseDay
{
    protected override string DayName => "4";

    protected override Task Part1(IEnumerable<string> lines)
    {
        var sum = lines
            .Select(line => NumWinsOnCard(line.Split(':')[1]))
            .Where(numWinning => numWinning > 0)
            .Sum(numWinning => (int)Math.Pow(2, numWinning - 1));

        Console.WriteLine(sum);

        return Task.CompletedTask;
    }

    protected override Task Part2(IEnumerable<string> lines)
    {
        var cardCopyCounts = new Dictionary<int, int>();

        void IncrementCardCount(int key)
        {
            if (!cardCopyCounts!.TryAdd(key, 1)) cardCopyCounts[key]++;
        }
        
        foreach (var line in lines)
        {
            var cardNumber = int.Parse(line.SkipWhile(c=>!char.IsDigit(c)).TakeWhile(char.IsDigit).ToArray());
            
            // Be sure to include the original card
            IncrementCardCount(cardNumber);
            
            var numWinning = NumWinsOnCard(line.Split(':')[1]);
            
            // Iterate through all the versions of this card we already have
            for (var i = 0; i < cardCopyCounts[cardNumber]; i++)
            {
                // Add in the newly won cards
                for (var j = 1; j <= numWinning; j++)
                {
                    IncrementCardCount(cardNumber + j);
                }
            }
        }

        Console.WriteLine(cardCopyCounts.Values.Sum());

        return Task.CompletedTask;
    }

    private static int NumWinsOnCard(string card)
    {
        var split = card.Split('|');
        var wins = SplitNumbers(split[0]);
        var ours = SplitNumbers(split[1]);

        return ours.Intersect(wins).Count();
    }

    private static IEnumerable<int> SplitNumbers(string input) =>
        input.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();
    
    internal static async Task<int> Main(string[] args)
    {
        return await new Day04().RunMain(args);
    }
}