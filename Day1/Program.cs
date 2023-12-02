using AdventOfCode2023;

namespace Day1;

internal class Day1 : BaseDay
{
    private static readonly Dictionary<string, int> DigitMapping = new()
    {
        { "0", 0 },
        { "1", 1 },
        { "2", 2 },
        { "3", 3 },
        { "4", 4 },
        { "5", 5 },
        { "6", 6 },
        { "7", 7 },
        { "8", 8 },
        { "9", 9 }
    };
    
    private static readonly Dictionary<string, int> WordDigitMapping = new()
    {
        { "zero",  0 },
        { "one",   1 },
        { "two",   2 },
        { "three", 3 },
        { "four",  4 },
        { "five",  5 },
        { "six",   6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine",  9 }
    };
    
    protected override string DayName => "1";

    protected override Task Part1(IEnumerable<string> lines)
    {
        Console.WriteLine($"Part 1 answer: {Common(lines, DigitMapping)}");
        return Task.CompletedTask;
    }

    protected override Task Part2(IEnumerable<string> lines)
    {
        Console.WriteLine(
            $"Part 2 answer: {Common(lines, DigitMapping.Concat(WordDigitMapping).ToDictionary())}");
        return Task.CompletedTask;
    }

    private static int Common(IEnumerable<string> lines, Dictionary<string, int> entries)
    {
        var sum = 0;
        foreach (var l in lines)
        {
            var line = l;
            int? firstDigit = null;
            do
            {
                foreach (var entry in entries.Where(entry => line.StartsWith(entry.Key)))
                {
                    firstDigit = entry.Value;
                }

                if (firstDigit is null) line = line[1..];
            } while (firstDigit is null && line.Length > 0);
            
            int? lastDigit = null;
            do
            {
                foreach (var entry in entries.Where(entry => line.EndsWith(entry.Key)))
                {
                    lastDigit = entry.Value;
                }

                if (lastDigit is null) line = line[..^1];
            } while (lastDigit is null && line.Length > 0);

            if (firstDigit is null || lastDigit is null) continue;
            var num = firstDigit * 10 + lastDigit;
            sum += num.Value;
        }
        
        return sum;
    }

    internal static async Task<int> Main(string[] args)
    {
        return await new Day1().RunMain(args);
    }
}