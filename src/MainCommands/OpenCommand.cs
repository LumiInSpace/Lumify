using Lumify.src;
using Lumify.src.Interfaces;
using Lumify.src.Models;
using Lumify.src.Utilities;
using System.Text.Json;

namespace Lumify.src.MainCommands
{
    public class OpenCommand : IMainCommand
    {
        public string Name => "open";
        public string Description => "Öffnet ein bestehendes Projekt: open <name>";

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"| {Emojis.Warning} | Bitte Projektnamen angeben. Verwendung: open <name>");
                return;
            }

            string name = args[0];
            string filePath = Path.Combine(GlobalVariables.MaterialListPath, $"{name}.lumify");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"| {Emojis.Cross} | Projekt nicht gefunden.");
                return;
            }

            string json = File.ReadAllText(filePath);
            var list = JsonSerializer.Deserialize<MaterialList>(json) ?? new MaterialList(name);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"| {Emojis.Check} | Projekt '{name}' geöffnet.");
            Console.ResetColor();

            ListHandler.Run(list, filePath);
        }
    }
}