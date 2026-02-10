using Lumify.src.Interfaces;
using Lumify.src.Models;
using Lumify.src.Utilities;

namespace Lumify.src;

public class ListCommandManager
{
    private readonly Dictionary<string, IListCommand> _commands = new();

    public void Register(IListCommand command)
    {
        _commands[command.Name.ToLower()] = command;
    }

    public bool Execute(string input, MaterialList list, string basePath)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string cmdName = parts[0].ToLower();

        if (_commands.TryGetValue(cmdName, out var cmd))
        {
            cmd.Execute(parts, list, basePath);
            return true;
        }
        else
        {
            Console.WriteLine($"| {Emojis.Cross} | Unknown command. Type 'help' for a list.");
            return false;
        }
    }
    
    public void ShowHelp()
    {
        Console.WriteLine($"{Emojis.List} List Commands");
        Console.WriteLine(new string('─', 48));
        Console.WriteLine("help   Show this list");
        Console.WriteLine("back   Close the material list");

        foreach (var command in _commands.Values.OrderBy(c => c.Name))
        {
            Console.WriteLine($"{command.Name.PadRight(6)} {command.Description}");
        }

        Console.WriteLine(new string('─', 48));
    }
}
