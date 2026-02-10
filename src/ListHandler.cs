using Lumify.src.Interfaces;
using Lumify.src.Models;
using Lumify.src.Navigation;
using Lumify.src.Utilities;

namespace Lumify.src;

public class ListCliService
{
    private readonly ListCommandManager _commandManager;
    private readonly CliNavigationService _navigation;

    public ListCliService(ListCommandManager commandManager, IEnumerable<IListCommand> commands, CliNavigationService navigation)
    {
        _commandManager = commandManager;
        _navigation = navigation;

        foreach (var command in commands)
        {
            _commandManager.Register(command);
        }
    }

    public void Run(MaterialList list, string materialListPath)
    {
        materialListPath = Path.GetFullPath(materialListPath);
        
        Console.Clear();
        
        while (true)
        {
            Console.WriteLine($"{Emojis.OpenFolder} Project '{list.Name}' opened. | Entries: {list.Items.Count}");
            Console.WriteLine("add <material> <amount> | remove <material> | show | save | back | start");
            Console.WriteLine("Type 'help' for details.");
             
            Console.Write("\nlist> ");
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(input))
                continue;


            if (input == "back")
                break;

            if (input == "start")
            {
                _navigation.RequestStart();
                return;
            }

            if (input == "help")
                _commandManager.ShowHelp();
            else
                _commandManager.Execute(input, list, materialListPath);

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
