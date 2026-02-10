using Lumify.src.Application.Contracts;
using Lumify.src.Utilities;

namespace Lumify.src
{
    public class ImportHandler
    {
        private readonly IImportService _importService;
        private readonly IProjectService _projectService;

        public ImportHandler(IImportService importService, IProjectService projectService)
        {
            _importService = importService;
            _projectService = projectService;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Import Module");
                Console.WriteLine("Supported file formats: .litematic"); //TODO integrate more formats
                Console.WriteLine("import <pfad> | help | back");
                Console.Write("\nimport> ");
                string? input = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                string[] parts = input.Split(' ', count: 2);
                string command = parts[0].ToLower();

                if (input == "help")
                {
                    GetHelp();
                }
                else if (input == "back")
                {
                    return;
                }
                else if (command == "import")
                {
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Invalid command. Usage: import <path>");
                        continue;
                    }
                    string cleanPath = string.Join(" ", parts.Skip(1)).Trim('"');
                    var itemList = _importService.ImportFromFile(cleanPath, out string message);
                    if (itemList != null)
                    {
                        Evaluate(itemList);
                    }
                    else
                    {
                        Console.WriteLine($"| {Emojis.Cross} | {message}");
                        continue;
                    }

                    Console.WriteLine("\nPress ENTER to continue...");
                    Console.ReadLine();
                }
            }
        }

        public void GetHelp()
        {
            Console.WriteLine("back: Return to the main menu");
            Console.WriteLine("import <path>: Import the selected file");
        }

        private void Evaluate(Dictionary<string, int>? itemList)
        {
            bool createList;
            bool removeTag = false;

            if (itemList == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"| {Emojis.Cross} | Items could not be extracted or the list is empty");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Extrahierte Items:");
            Console.ResetColor();
            Console.WriteLine();

            foreach (var item in itemList)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }

            Console.WriteLine();

            createList = AskYesNoHandler.AskYesNo($"{Emojis.List} Create a list from extracted materials?");
            if (createList)
            {
                removeTag = AskYesNoHandler.AskYesNo("Remove item tag (e.g. minecraft:)?");
            }

            CreateProjectFromImport(itemList, createList, removeTag);
        }

        private void CreateProjectFromImport(Dictionary<string, int> itemList, bool createList, bool removeTag)
        {
            if (!createList)
            {
                return;
            }

            if (removeTag)
            {
                var newDict = new Dictionary<string, int>();

                foreach (var item in itemList)
                {
                    var parts = item.Key.Split(':');
                    var newKey = parts.Length > 1 ? parts[1] : "";
                    newDict[newKey] = item.Value;
                }

                itemList = newDict;
            }

            string name;

            while (true)
            {
                Console.Write("Name: ");
                name = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                bool created = _projectService.TryCreate(name, itemList, out string message, out _);
                if (!created)
                {
                    Console.WriteLine($"| {Emojis.Warning} | {message}");
                    continue;
                }

                Console.WriteLine($"| {Emojis.Check} | {message}");
                break;
            }
        }
    }
}
