using System.CommandLine;
using System.Diagnostics;
using Spectre.Console;

namespace AdventOfCode2023;

/// <summary>
/// The template for a day. Includes command-line parsing and file reading line by line. Simply inherit this class and
/// provide the runtime a <c>static Main</c> method. 
/// </summary>
public abstract class BaseDay
{
    /// <summary>
    /// Which day this is, for example "1".
    /// </summary>
    protected virtual string DayName => "NO NAME";
    
    /// <summary>
    /// Implement the solution to part 1 here!
    /// </summary>
    /// <param name="lines">The lines from the input/example</param>
    /// <returns>Nada</returns>
    protected abstract Task Part1(IEnumerable<string> lines);
    
    /// <summary>
    /// Implement the solution to part 2 here!
    /// </summary>
    /// <param name="lines">The lines from the input/example</param>
    /// <returns>Nada</returns>
    protected abstract Task Part2(IEnumerable<string> lines);
    
    /// <summary>
    /// Call this from <c>static Main</c>.
    /// </summary>
    /// <param name="args">argv</param>
    /// <returns>Exit code</returns>
    protected async Task<int> RunMain(string[] args)
    {
        var fileOpt = new Option<FileInfo?>(name: "--file", "Input file to use, if not using stdin");
        fileOpt.AddAlias("-f");

        var rootCommand = new RootCommand($"Day {DayName}");
        rootCommand.AddGlobalOption(fileOpt);

        var part1Command = new Command("part1");
        part1Command.AddAlias("p1");
        part1Command.AddAlias("1");
        part1Command.SetHandler(file => Handle(file, Part1Wrapper), fileOpt);
        rootCommand.AddCommand(part1Command);

        var part2Command = new Command("part2");
        part2Command.AddAlias("p2");
        part2Command.AddAlias("2");
        part2Command.SetHandler(file => Handle(file, Part2Wrapper), fileOpt);
        rootCommand.AddCommand(part2Command);
        
        var bothPartsCommand = new Command("both-parts");
        bothPartsCommand.AddAlias("bothparts");
        bothPartsCommand.AddAlias("bp");
        bothPartsCommand.SetHandler(async file =>
        {
            await Handle(file, async lines =>
            {
                var enumerable = lines.ToList();
                await Part1Wrapper(enumerable);
                AnsiConsole.WriteLine();
                await Part2Wrapper(enumerable);
            });
        }, fileOpt);
        rootCommand.AddCommand(bothPartsCommand);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task Handle(FileInfo? file, Func<IEnumerable<string>, Task> next)
    {
        IEnumerable<string> lines;
        if (Console.IsInputRedirected)
        {
            lines = Console.In.ReadToEnd().Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l));
        }
        else
        {
            if (file is null || !file.Exists)
            {
                throw new ApplicationException("File does not exist");
            }

            lines = File.ReadLines(file.FullName);
        }

        AnsiConsole.WriteLine();
        await next(lines);
        AnsiConsole.WriteLine();
    }

    private async Task Part1Wrapper(IEnumerable<string> lines)
    {
        var rule = new Rule("Part 1")
        {
            Style = new Style(Color.Green),
            Justification = Justify.Left,
        };
        AnsiConsole.Write(rule);
        
        var stopwatch = Stopwatch.StartNew();
        await AnsiConsole.Status().StartAsync("Running...", async ctx =>
        {
            ctx.Spinner = Spinner.Known.Star;
            await Part1(lines);
        });
        //await Part1(lines);
        stopwatch.Stop();
        
        AnsiConsole.MarkupLine($"[lightslategrey]({stopwatch.ElapsedMilliseconds}ms)[/]");
    }
    
    private async Task Part2Wrapper(IEnumerable<string> lines)
    {
        var rule = new Rule("Part 2")
        {
            Style = new Style(Color.Green),
            Justification = Justify.Left,
        };
        AnsiConsole.Write(rule);

        var stopwatch = Stopwatch.StartNew();
        await Part2(lines);
        stopwatch.Stop();
        
        AnsiConsole.MarkupLine($"[lightslategrey]({stopwatch.ElapsedMilliseconds}ms)[/]");
    }
}