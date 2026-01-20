using Lumify.src.Models;
using Lumify.src.Interfaces;
using Lumify.src.Utilities;
using System.Text.Json;

namespace Lumify.src.MainCommands
{
    public class NewCommand : IMainCommand
    {
        public string Name => "new";
        public string Description => "Erstellt eine neue Materialliste: new <name>";

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"| {Emojis.Warning} | Bitte Namen angeben. Verwendung: new <name>");
                return;
            }

            var name = args[0];
            var filePath = Path.Combine(GlobalVariables.MaterialListPath, $"{name}.lumify");

            if (File.Exists(filePath))
            {
                Console.WriteLine($"| {Emojis.Warning} | Projekt existiert bereits.");
                return;
            }

            var list = new MaterialList(name);
            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);

            Console.WriteLine($"| {Emojis.Check} | Projekt '{name}' erstellt!");
        }
    }
}