using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using AdventOfCode2023;

using NodeCollection = System.Collections.Generic.Dictionary<string, (string, string)>;

namespace Day08;

internal class Day08 : BaseDay
{
    protected override string DayName => "8";
    
    protected override Task Part1(IEnumerable<string> lines)
    {
        var (directions, nodes) = ParseFile(lines.ToList());

        var numSteps = 0;
        var currentNode = "AAA";
        while (true)
        {

            if (currentNode == "ZZZ") break;

            currentNode = GetNextNode(nodes, currentNode, directions[numSteps % directions.Length]);

            numSteps += 1;
        }
        
        Console.WriteLine(numSteps);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Note: I don't like how AoC just assumes an LCM method will work despite that assumption not being
    /// stated anywhere in the problem description.
    /// </summary>
    protected override Task Part2(IEnumerable<string> lines)
    {
        var (directions, nodes) = ParseFile(lines.ToList());

        var startingNodes = nodes.Select(n => n.Key).Where(n => n[^1] == 'A').ToList();
        var cycleLengths = new ConcurrentBag<int>();
        Parallel.ForEach(startingNodes, node =>
        {
            var currentNode = node;
            var steps = 0;
            while (currentNode[^1] != 'Z')
            {
                currentNode = GetNextNode(nodes, currentNode, directions[steps % directions.Length]);
            
                steps += 1;
            }
            
            cycleLengths.Add(steps);
        });

        var lcm = cycleLengths.Aggregate(1L, (current, length) => Lcm(current, length));
        Console.WriteLine(lcm);

        return Task.CompletedTask;
    }

    private static (char[], NodeCollection) ParseFile(IReadOnlyList<string> lines)
    {
        var directions = lines.First().ToCharArray();

        var nodes = lines.Skip(1).Select(node => NodeLinePattern.Match(node))
            .ToDictionary(
                match => match.Groups["node"].Value,
                match => (match.Groups["left"].Value, match.Groups["right"].Value)
            );

        return (directions, nodes);
    }

    private static string GetNextNode(NodeCollection nodes, string currentNode, char direction)
    {
        return direction switch
        {
            'L' => nodes[currentNode].Item1,
            'R' => nodes[currentNode].Item2,
            _ => throw new Exception("Unknown direction")
        };
    }

    private static readonly Regex NodeLinePattern = new(@"(?<node>\w{3}) = \((?<left>\w{3}), (?<right>\w{3})\)");

    // Shamelessly grabbed from https://stackoverflow.com/a/20824923
    private static long Gcf(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static long Lcm(long a, long b)
    {
        return a / Gcf(a, b) * b;
    }

    internal static async Task<int> Main(string[] args)
    {
        return await new Day08().RunMain(args);
    }
}