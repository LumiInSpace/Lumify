using Lumify.Interfaces;
using Lumify.Models;
using Lumify.Utilities;
using System.Text.Json;

namespace Lumify.MainCommands
{
    public class OpenCommand : IMainCommand
    {
        public string Name => "open";
        public string Description => "Öffnet ein bestehendes Projekt: open <name>";

        public void Execute(string basePath, string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"| {Emojis.Warning} | Bitte Projektnamen angeben. Verwendung: open <name>");
                return;
            }

            string name = args[0];
            string filePath = Path.Combine(basePath, $"{name}.lumify");

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