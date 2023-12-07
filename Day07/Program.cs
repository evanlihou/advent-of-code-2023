using AdventOfCode2023;

namespace Day07;

internal class Day07 : BaseDay
{
    protected override string DayName => "7";
    
    protected override Task Part1(IEnumerable<string> lines)
    {
        var hands = lines.Select(l =>
        {
            var tokens = l.Split(' ');
            return new Hand(tokens[0].ToCharArray(), ulong.Parse(tokens[1]));
        });

        var handRank = hands.OrderBy(h => GetHandStrength(h.Cards))
            .ThenByDescending(h => h.Cards, new HandComparer(CardWeights)).ToList();

        Console.WriteLine(GetMultipliedBets(handRank));
        return Task.CompletedTask;
    }

    protected override Task Part2(IEnumerable<string> lines)
    {
        var hands = lines.Select(l =>
        {
            var tokens = l.Split(' ');
            return new Hand(tokens[0].ToCharArray(), ulong.Parse(tokens[1]));
        });
        var handRank = hands.OrderBy(h => GetHandStrength(h.Cards, true))
            .ThenByDescending(h => h.Cards, new HandComparer(CardWeightsJoker)).ToList();

        Console.WriteLine(GetMultipliedBets(handRank));
        return Task.CompletedTask;
    }

    private static uint GetHandStrength(char[] hand, bool replaceJokers = false)
    {
        while (true)
        {
            if (replaceJokers)
            {
                var jokerIndexes = new List<int>();
                var i = -1;
                while (true)
                {
                    i = Array.IndexOf(hand, 'J', i + 1);
                    if (i < 0) break;
                    jokerIndexes.Add(i);
                }

                if (jokerIndexes.Count == 0)
                {
                    replaceJokers = false;
                    continue;
                }

                var maxStrength = 0u;
                var newHand = new char[hand.Length];
                Array.Copy(hand, newHand, hand.Length);
                foreach (var combo in GetCombinationsWithReplacementLexographicOrder(CardsWithoutJoker, jokerIndexes.Count))
                {
                    for (var j = 0; j < jokerIndexes.Count; j++)
                    {
                        newHand[jokerIndexes[j]] = combo[j];
                    }

                    var strength = GetHandStrength(newHand);
                    if (strength > maxStrength)
                    {
                        maxStrength = strength;
                    }
                }
                
                return maxStrength;
            }

            var handDistribution = hand.GroupBy(c => c).ToList();

            if (handDistribution.Count == 1) return 7;
            if (handDistribution.Select(g => g.Count()).Max() == 4) return 6;
            if (handDistribution.Count == 2) return 5;
            if (handDistribution.Select(g => g.Count()).Max() == 3) return 4;
            if (handDistribution.Select(g => g.Count()).Count(c => c == 2) == 2) return 3;
            if (handDistribution.Select(g => g.Count()).Max() == 2) return 2;
            return 1;
        }
    }

    private long GetMultipliedBets(IReadOnlyList<Hand> hands)
    {
        var sum = 0L;
        for (var i = hands.Count; i > 0; i--)
        {
            var hand = hands[i - 1];
            sum += i * (long)hand.Bet;
        }

        return sum;
    }

    private static readonly char[] CardWeights = { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
    private static readonly char[] CardWeightsJoker = { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };
    private static readonly char[] CardsWithoutJoker = CardWeightsJoker[..^1];

    private static IEnumerable<List<T>> GetCombinationsWithReplacementLexographicOrder<T>(IList<T> pool, 
        int comboLength)
    {
        if (comboLength <= 0) throw new ArgumentOutOfRangeException(nameof(comboLength));
        foreach (var list in GetCombinations(pool, comboLength).Select(c => 
                     c.ToList()))
            yield return list;
    }

    private static IEnumerable<IEnumerable<T>> GetCombinations<T>(IList<T> list, int length)
    {
            
        if (length == 1) return list.Select(t => new[] { t });

        return GetCombinations(list, length - 1).SelectMany(t => list, (t1, t2) => t1.Concat(new[] { t2 
        }));
    }

    private class HandComparer : IComparer<IEnumerable<char>>
    {
        private readonly char[] _weights;

        public HandComparer(char[] weights)
        {
            _weights = weights;
        }

        public int Compare(IEnumerable<char>? x, IEnumerable<char>? y)
        {
            foreach (var (xc, yc) in x!.Zip(y!))
            {
                if (xc == yc) continue;
                return Array.IndexOf(_weights, xc).CompareTo(Array.IndexOf(_weights, yc));
            }

            return 0;
        }
    }

    internal static async Task<int> Main(string[] args)
    {
        return await new Day07().RunMain(args);
    }
}

internal record Hand(char[] Cards, ulong Bet);