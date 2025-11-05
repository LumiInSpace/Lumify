using Lumify.Interfaces;
using Lumify.Models;
using System.Text.Json;

namespace Lumify.MainCommands
{
    public class NewCommand : IMainCommand
    {
        public string Name => "new";
        public string Description => "Erstellt eine neue Materialliste: new <name>";

        public void Execute(string basePath, string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("| ⚠️ | Bitte Namen angeben. Verwendung: new <name>");
                return;
            }

            var name = args[0];
            var filePath = Path.Combine(basePath, $"{name}.json");

            if (File.Exists(filePath))
            {
                Console.WriteLine("| ⚠️ | Projekt existiert bereits.");
                return;
            }

            var list = new MaterialList(name);
            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);

            Console.WriteLine($"| ✅ | Projekt '{name}' erstellt!");
        }
    }
}