using Lumify.Imports;
using Lumify.Models;
using Lumify.Utilities;
using System.Text.RegularExpressions;

namespace Lumify
{
    public class ImportHandler
    {
        public static void Run()
        {


            ImportCommandManager manager = new();
            manager.Register(new LitematicaImportCommand());
            //TODO mehr Import Formate

            while (true)
            {

                Console.Clear();
                Console.WriteLine("Unterstützte Datei-Formate: .litematic"); //TODO weitere Formate einbinden
                Console.WriteLine("'help' für Hilfe");
                Console.Write("\n>");
                string? input = Console.ReadLine()?.Trim();
                if (String.IsNullOrWhiteSpace(input)) { continue; }
                string[] parts = input.Split(' ', count: 2);
                string command = parts[0].ToLower();

                if (input == "help")
                {
                    GetHelp();
                }
                else if (input == "back") { return; }
                else if (command == "import")
                {
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Befehl nicht korrekt. Verwendung: import <pfad>");
                        continue;
                    }
                    string cleanPath = parts[1].Replace('"', ' ').Trim();
                    var itemList = manager.Execute(cleanPath);
                    if (itemList != null)
                    {
                        Evalute(itemList);
                    }
                    else
                    {
                        Console.WriteLine("Kein gültiger Befehl. Bitte erneut eingeben!");
                        continue;
                    }

                    Console.WriteLine("\nDrücke ENTER um fortzufahren...");
                    Console.ReadLine();
                }
            }
        }

        public static void GetHelp()
        {
            Console.WriteLine("back: Zurück zum Hauptmenü");
            Console.WriteLine("import <pfad>: Importiert die ausgewählte Datei");
        }

        private static void Evalute(Dictionary<string, int>? itemList)
        {
            bool createList;
            bool removeTag = false;

            if (itemList == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("| ❌ | Items konnten nicht extrahiert werden oder die Liste ist leer");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor= ConsoleColor.Magenta;
            Console.WriteLine("Extrahierte Items:");
            Console.ResetColor();
            Console.WriteLine();

            foreach (var item in itemList)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }

            Console.WriteLine();

            createList = AskYesNoHandler.AskYesNo("Liste aus extrahierten Materialien erstellen?");
            if (createList)
            {
                removeTag = AskYesNoHandler.AskYesNo("Item Tag (z.B minecraft:) entfernen?");
            }

            Convert(itemList, createList, removeTag);
        }

        private static void Convert(Dictionary<string, int> itemList, bool createList, bool removeTag)
        {
            if (createList == false)
                return;

            if (removeTag == true)
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

            Console.Write("Name: ");
            string? name;
            while (true) 
            {
                name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name)) break;
            }
            
            new MaterialList(name, itemList);

        }
    }
}