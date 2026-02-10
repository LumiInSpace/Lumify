using Lumify.src.Interfaces;
using Lumify.src.Models;
using Lumify.src.Utilities;

namespace Lumify.src;

public class ListHandler
{
    private readonly ListCommandManager _commandManager;

    public ListHandler(ListCommandManager commandManager, IEnumerable<IListCommand> commands)
    {
        _commandManager = commandManager;

        foreach (var command in commands)
        {
            _commandManager.Register(command);
        }
    }

    public void Run(MaterialList list, string materialListPath)
    {
        //TODO more commands
        
        materialListPath = Path.GetFullPath(materialListPath);
        
        Console.Clear();
        
        while (true)
        {
            Console.WriteLine($"{Emojis.OpenFolder} Project '{list.Name}' opened. | Entries: {list.Items.Count}");
            Console.WriteLine("add <material> <amount> | remove <material> | show | save | back");
            Console.WriteLine("Type 'help' for details.");
             
            Console.Write("\nlist> ");
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(input))
                continue;


            if (input == "back")
                break;

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
