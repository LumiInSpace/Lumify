using Lumify.Models;
using System.Text;
using System.Text.Json;

namespace Lumify
{
    class Program
    {
        private const string MaterialListsPath = @"C:\ProgramData\Lumify\Lists";

        static void Main(string[] args)
        {
            Console.Title = "Lumify";
            
            Console.OutputEncoding = Encoding.UTF8;
            
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("| ✅ | Lumify gestartet.");
            Console.ResetColor();
            
            int currentCodePage = Console.OutputEncoding.CodePage;
            if (currentCodePage != 65001)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("| ⚠️ | Es ist keine UTF-8 Codierung aktiviert! Manche Zeichen könnten fehlerhaft dargestellt werden.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Green;
                Console.WriteLine("| ✅ | UTF-8 Codierung verfügbar. ^_~");
                Console.ResetColor();
            }
            
            Console.WriteLine("\n");

            if (!Directory.Exists(MaterialListsPath))
            {

                Directory.CreateDirectory(MaterialListsPath);
            }
            //----
            //TODO Ladebalken einfügen beim Laden von Material-Listen
            //Load Material Lists
            //----
            string[] availableCommands = { "new", "show", "open", "exit" };
            Console.WriteLine("Verfügbare Befehle: new <name>, show, open <name>, exit");

            while (true)
            {
                Console.Write("\n>");
                string? input = Console.ReadLine()?.Trim();
                Console.WriteLine();

                if (string.IsNullOrWhiteSpace(input)) continue;
                string[] parts = input.Split(' ');
                string command = parts[0].ToLower();

                switch (command)
                {
                    case "new":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Dieser Befehl ist ungültig. Gebe einen gültigen Name ein: new <name>");
                            break;
                        }
                        if (availableCommands.Contains(parts[1]))
                        {
                            Console.WriteLine("Dieser Befehl ist ungültig. Gebe einen gültigen Name ein: new <name>");
                            break;
                        }
                        CreateNewProject(parts[1]);
                        break;

                    case "show":
                        ListProjects();
                        break;

                    case "open":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Dieser Befehl ist ungültig. Gebe einen gültigen Name ein: open <name>");
                            break;
                        }
                        if (availableCommands.Contains(parts[1]))
                        {
                            Console.WriteLine("Dieser Befehl ist ungültig. Gebe einen gültigen Name ein: open <name>");
                            break;
                        }
                        OpenProject(parts[1]);
                        break;

                    case "exit":
                        return;

                    default:
                        Console.WriteLine("Dieser Befehl existiert nicht");
                        break;
                }
            }
        }


        static private void CreateNewProject(string name)
        {
            string filePath = Path.Combine(MaterialListsPath, $"{name}.json");

            if (File.Exists(filePath))
            {
                Console.WriteLine("| ⚠️ | Projekt existiert bereits.");
                return;
            }

            var list = new MaterialList(name);
            SaveProject(list);
            Console.WriteLine($"| ✅ | Projekt '{name}' erstellt!");
        }

        static private void SaveProject(MaterialList list)
        {
            string filePath = Path.Combine(MaterialListsPath, $"{list.Name}.json");
            string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        static private void OpenProject(string name)
        {
            string filePath = Path.Combine(MaterialListsPath, $"{name}.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("| ❌ | Projekt nicht gefunden.");
                return;
            }

            string json = File.ReadAllText(filePath);

            var list = JsonSerializer.Deserialize<MaterialList>(json) ?? new MaterialList(name);

            ListHandler.Run(list, filePath);
        }

        static void ListProjects()
        {
            var files = Directory.GetFiles(MaterialListsPath, "*.json");
            if (files.Length == 0)
            {
                Console.WriteLine("Keine Projekte gefunden.");
                return;
            }

            Console.WriteLine("📜 Projekte:");
            foreach (var f in files)
            {
                Console.WriteLine(" - " + Path.GetFileNameWithoutExtension(f));
            }
        }
    }
}
