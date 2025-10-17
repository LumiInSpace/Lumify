using System.Text.Json.Nodes;
using Lumify.Commands;
using Lumify.Core.Commands;
using Lumify.Models;

namespace Lumify;

public class ListHandler
{
    public static void Run(MaterialList list, string materialListPath)
    {
        var commandManager = new CommandManager();
        
        commandManager.Register(new AddCommand());
        commandManager.Register(new ShowCommand());
        commandManager.Register(new SaveCommand());
        //TODO more commands
        
        materialListPath = Path.GetFullPath(materialListPath);
        
        Console.Clear();
        
        while (true)
        {
            Console.WriteLine($"📂 Projekt '{list.Name}' geöffnet.");
            Console.WriteLine("Tippe 'help' für eine Befehlsliste.");
            
            Console.Write("\n> ");
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "back")
                break;

            if (input == "help")
                commandManager.ShowHelp();
            else
                commandManager.Execute(input, list, materialListPath);
            
            Console.ReadLine();
            Console.Clear();
        }
    }
}