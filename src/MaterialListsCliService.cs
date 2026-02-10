using Lumify.src.Configuration;
using Lumify.src.Interfaces;
using Lumify.src.Navigation;
using Lumify.src.Utilities;
using Microsoft.Extensions.Options;

namespace Lumify.src;

public class MaterialListsCliService : IToolCliService
{
    private readonly MainCommandManager _commandManager;
    private readonly LumifyOptions _options;
    private readonly CliNavigationService _navigation;

    public MaterialListsCliService(
        MainCommandManager commandManager,
        IEnumerable<IMainCommand> commands,
        IOptions<LumifyOptions> options,
        CliNavigationService navigation)
    {
        _commandManager = commandManager;
        _options = options.Value;
        _navigation = navigation;

        foreach (var command in commands)
        {
            _commandManager.Register(command);
        }
    }

    public int MenuNumber => 1;
    public string MenuLabel => "Materiallisten";

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            PrintWelcome();

            Console.Write("\nmaterials> ");
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            if (input == "back" || input == "start")
            {
                return;
            }

            if (input == "help")
            {
                _commandManager.ShowHelp();
            }
            else
            {
                _commandManager.Execute(input);

                if (_navigation.ConsumeStartRequest())
                {
                    return;
                }
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }

    private void PrintWelcome()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{_options.AppName} - Materiallisten");
        Console.ResetColor();
        Console.WriteLine("Manage material lists directly in the terminal.");
        Console.WriteLine();
        Console.WriteLine($"{Emojis.List} Commands");
        Console.WriteLine(new string('-', 48));

        foreach (var command in _commandManager.GetCommands())
        {
            Console.WriteLine($"{command.Name.PadRight(6)} {command.Description}");
        }

        Console.WriteLine("help   Show all commands");
        Console.WriteLine("back   Return to start screen");
        Console.WriteLine(new string('-', 48));
    }
}
