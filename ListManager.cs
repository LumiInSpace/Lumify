using Lumify.Models;
using Lumify.Interfaces;

namespace Lumify;

public class CommandManager
{
    private readonly Dictionary<string, IUserCommand> _commands = new();

    public void Register(IUserCommand command)
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
            Console.WriteLine("| ❌ | Unbekannter Befehl. Tippe 'help' für eine Liste.");
            return false;
        }
    }
    
    public void ShowHelp()
    {
        Console.WriteLine("📜 Verfügbare Befehle:");
        Console.WriteLine("- help: Zeigt diese Liste an.");
        Console.WriteLine("- back: Schließt die Materialliste");
        foreach (var c in _commands.Values)
            Console.WriteLine($"- {c.Name}: {c.Description}");
    }
}