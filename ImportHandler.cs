using Lumify.Imports;

namespace Lumify
{
    public class ImportHandler
    {
        public static void Run()
        {
            
            
            ImportManager manager = new();
            manager.Register(new LitematicaImportCommand());
            //TODO mehr Imort Formate

            while (true) {

                Console.Clear();
                Console.WriteLine("Unterstützte Datei-Formate: .litematic"); //TODO weitere Formate einbinden
                Console.WriteLine("'help' für Hilfe");
                Console.Write("\n>");
                string? input = Console.ReadLine()?.Trim().ToLower();
                if (String.IsNullOrWhiteSpace(input)) { continue; }
                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (input == "help")
                {
                    GetHelp();
                }
                else if (input == "back") { return; }
                else if (parts[0] == "import")
                {
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Befehl nicht korrekt. Verwendung: import <pfad>");
                        continue;
                    }
                    string cleanPath = parts[1].Replace('"', ' ').Trim();
                    manager.Execute(cleanPath);
                }
                else
                {
                    Console.WriteLine("Kein gültiger Befehl. Bitte erneut eingeben!");
                    continue;
                }

                Console.ReadLine();
            }
        }

        public static void GetHelp()
        {
            Console.WriteLine("back: Zurück zum Hauptmenü");
            Console.WriteLine("import <pfad>: Importiert die ausgewählte Datei");
        }
    }
}