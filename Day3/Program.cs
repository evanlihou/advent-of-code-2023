using System.Text.RegularExpressions;
using AdventOfCode2023;

namespace Day3;

internal class Day3 : BaseDay
{
    protected override string DayName => "3";
    
    protected override Task Part1(IEnumerable<string> lines)
    {
        var sum = 0;
        
        var linesArr = lines.ToArray();
        var lineLength = linesArr[0].Length;
        for (var i = 0; i < linesArr.Length; i++)
        {
            var matches = DigitPattern.Matches(linesArr[i]);
            foreach (Match match in matches)
            {
                var hasSymbol = false;
                var startIndex = Math.Max(MatchStart(match) - 1, 0);
                var endIndex = Math.Min(MatchEnd(match) + 1, lineLength - 1);
                foreach (var l in new[] { i - 1, i, i + 1 })
                {
                    if (l < 0 || l >= linesArr.Length) continue;
                    if (linesArr[l][startIndex..(endIndex+1)].Any(c => !char.IsDigit(c) && c != '.')) hasSymbol = true;
                }
                if (hasSymbol) sum += int.Parse(match.Value);
            }
        }
        
        Console.WriteLine(sum);

        return Task.CompletedTask;
    }

    protected override Task Part2(IEnumerable<string> lines)
    {
        var sum = 0;

        var linesArr = lines as string[] ?? lines.ToArray();
        var numbers = linesArr.Select(line => DigitPattern.Matches(line).ToArray()).ToList();

        for (var i = 0; i < linesArr.Length; i++)
        {
            foreach (var star in linesArr[i].Select((c, idx) => c == '*' ? idx : -1).Where(idx => idx != -1).ToArray())
            {
                
                var nearbyNumbers = new List<int>();
                foreach (var l in new[] { i - 1, i, i + 1 })
                {
                    if (l < 0 || l >= linesArr.Length) continue;
                    var matches = numbers[l].Where(m => MatchEnd(m) >= star - 1 && MatchStart(m) <= star + 1);
                    nearbyNumbers.AddRange(matches.Select(m => int.Parse(m.Value)));
                }

                if (nearbyNumbers.Count == 2) sum += nearbyNumbers.Aggregate((a, b) => a * b);
            }
        }
        
        Console.WriteLine(sum);

        return Task.CompletedTask;
    }

    private static int MatchStart(Capture match) => match.Index;
    private static int MatchEnd(Capture match) => match.Index + match.Length - 1;

    private static readonly Regex DigitPattern = new(@"[0-9]+");

    internal static async Task<int> Main(string[] args)
    {
        return await new Day3().RunMain(args);
    }
}