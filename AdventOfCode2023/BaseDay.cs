using System.CommandLine;

namespace AdventOfCode2023;

public abstract class BaseDay
{
    protected virtual string DayName => "NO NAME";
    protected async Task<int> RunMain(string[] args)
    {
        var fileOpt = new Option<FileInfo?>(name: "--file");
        fileOpt.AddAlias("-f");

        var rootCommand = new RootCommand($"Day {DayName}");
        rootCommand.AddGlobalOption(fileOpt);

        var part1Command = new Command("part1");
        part1Command.SetHandler(file => ReadFile(file, Part1), fileOpt);
        rootCommand.AddCommand(part1Command);
        
        var part2Command = new Command("part2");
        part2Command.SetHandler(file => ReadFile(file, Part2), fileOpt);
        rootCommand.AddCommand(part2Command);
        
        var bothPartsCommand = new Command("bothparts");
        bothPartsCommand.SetHandler(file =>
        {
            ReadFile(file, Part1);
            ReadFile(file, Part2);
        }, fileOpt);
        rootCommand.AddCommand(bothPartsCommand);

        return await rootCommand.InvokeAsync(args);
    }

    private static void ReadFile(FileInfo? file, Func<IEnumerable<string>, Task> next)
    {
        if (file is null || !file.Exists)
        {
            throw new ApplicationException("File does not exist");
        }

        next(File.ReadLines(file.FullName));
    }

    protected abstract Task Part1(IEnumerable<string> lines);
    protected abstract Task Part2(IEnumerable<string> lines);
}