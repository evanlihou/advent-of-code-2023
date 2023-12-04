using System.Text.RegularExpressions;
using AdventOfCode2023;

namespace Day02;

internal class Day02 : BaseDay
{
    protected override string DayName => "2";
    
    protected override Task Part1(IEnumerable<string> lines)
    {
        var games = ParseLines(lines);

        var impossibleGames = games.Where(g => g.Samples.All(s => 
            s is { Red: <= 12, Green: <= 13, Blue: <= 14 }));
        
        Console.WriteLine(impossibleGames.Sum(g => g.Id));

        return Task.CompletedTask;
    }

    protected override Task Part2(IEnumerable<string> lines)
    {
        var games = ParseLines(lines);

        var powers = games.Select(g =>
            g.Samples.Max(s => s.Red) * g.Samples.Max(s => s.Green) * g.Samples.Max(s => s.Blue));
        
        Console.WriteLine(powers.Sum());

        return Task.CompletedTask;
    }

    private static IEnumerable<Game> ParseLines(IEnumerable<string> lines)
    {
        return lines.Select(l =>
        {
            var match = LinePattern.Match(l);
            var samples = match.Groups["samples"].Value.Split("; ").Select(s =>
            {
                var sample = new Sample();
                var marbles = s.Split(", ");
                foreach (var marble in marbles)
                {
                    if (marble.Contains("red")) sample.Red = int.Parse(marble.Split(' ')[0]);
                    if (marble.Contains("green")) sample.Green = int.Parse(marble.Split(' ')[0]);
                    if (marble.Contains("blue")) sample.Blue = int.Parse(marble.Split(' ')[0]);
                }

                return sample;
            });
            return new Game(int.Parse(match.Groups["gameId"].Value), samples);
        });
    }

    private static readonly Regex LinePattern = new(@"^Game (?<gameId>\d+): (?<samples>.*)");

    private record Game(int Id, IEnumerable<Sample> Samples)
    {
        public readonly int Id = Id;
        public readonly IEnumerable<Sample> Samples = Samples;
    }

    private record Sample
    {
        public int Red;
        public int Green;
        public int Blue;
    }
    
    internal static async Task<int> Main(string[] args)
    {
        return await new Day02().RunMain(args);
    }
}