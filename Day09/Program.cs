using AdventOfCode2023;
using Spectre.Console;

namespace Day09;

internal class Day09 : BaseDay
{
    protected override string DayName => "9";
    
    protected override Task Part1(IEnumerable<string> lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            var numbers = line.Split(' ').Select(int.Parse).ToList();
            numbers.Reverse();
            sum += GetExtrapolation(numbers);
        }
        
        AnsiConsole.WriteLine(sum);
        return Task.CompletedTask;
    }

    protected override Task Part2(IEnumerable<string> lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            var numbers = line.Split(' ').Select(int.Parse).ToList();
            sum += GetExtrapolation(numbers);
        }
        
        AnsiConsole.WriteLine(sum);
        return Task.CompletedTask;
    }

    private static int GetExtrapolation(IEnumerable<int> numbers)
    {
        var lastAdds = new List<int>();
        var diffQueue = new Queue<int>(numbers);

        while (true)
        {
            var notAnswer = false;
            var lastNum = diffQueue.Dequeue();
            lastAdds.Add(lastNum);
            for (var i = 0; i < diffQueue.Count; i++)
            {
                var nextNum = diffQueue.Dequeue();
                var diff = lastNum - nextNum;
                if (diff != 0)
                {
                    notAnswer = true;
                }

                lastNum = nextNum;
                diffQueue.Enqueue(diff);
            }
                
            if (!notAnswer) break;
        }
            
        return lastAdds.Aggregate(0, (acc, num) => acc + num);
    }
    
    internal static Task<int> Main(string[] args)
    {
        return new Day09().RunMain(args);
    }
}