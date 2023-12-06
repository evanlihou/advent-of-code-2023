using AdventOfCode2023;

namespace Day06;

internal class Day06 : BaseDay
{
    protected override string DayName => "6";

    protected override Task Part1(IEnumerable<string> linesEnum)
    {
        var lines = linesEnum.ToArray();
        var times = lines[0].Split(':')[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse);
        var distances = lines[1].Split(':')[1].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse);

        var winArray = new List<int>();

        foreach (var (time, distance) in times.Zip(distances))
        {
            var numWins = 0;
            for (var t = 1; t < time; t++)
            {
                if (t * (time - t) > distance)
                {
                    numWins++;
                }
            }
            winArray.Add(numWins);
        }
        
        Console.WriteLine(winArray.Aggregate(1, (a, b) => a * b));
        return Task.CompletedTask;
    }
    
    protected override Task Part2(IEnumerable<string> linesEnum)
    {
        var lines = linesEnum.ToArray();
        var time = long.Parse(lines[0].Split(':')[1].Where(char.IsDigit).ToArray());
        var distance = long.Parse(lines[1].Split(':')[1].Where(char.IsDigit).ToArray());

        // Find the min...
        var min = 0L;
        for (var t = 1L; t < time; t++)
        {
            if (t * (time - t) <= distance) continue;
            min = t;
            break;
        }
        
        // And the max...
        var max = 0L;
        for (var t = time; t > min; t--)
        {
            if (t * (time - t) <= distance) continue;
            max = t;
            break;
        }
        
        Console.WriteLine(max - min + 1);
        return Task.CompletedTask;
    }

    internal static async Task<int> Main(string[] args)
    {
        return await new Day06().RunMain(args);
    }
}