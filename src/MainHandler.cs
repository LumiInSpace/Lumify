using Lumify.src.MainCommands;
using Lumify.src.Utilities;
using System.Text;

namespace Lumify.src
{
    public class MainHandler
    {

        public void Initialize()
        {
            Console.Title = "Lumify";

            Console.OutputEncoding = Encoding.UTF8;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"| {Emojis.Check} | Lumify gestartet.");
            Console.ResetColor();

            int currentCodePage = Console.OutputEncoding.CodePage;
            if (currentCodePage != 65001)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"| {Emojis.Warning} | Es ist keine UTF-8 Codierung aktiviert! Manche Zeichen könnten fehlerhaft dargestellt werden.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"| {Emojis.Check} | UTF-8 Codierung verfügbar.");
                Console.ResetColor();
            }

            Console.WriteLine("\n");

            if (!Directory.Exists(GlobalVariables.MaterialListPath))
            {
                Directory.CreateDirectory(GlobalVariables.MaterialListPath);
            }
        }

        public void Run()
        {
            var commandManager = new MainCommandManager();

            commandManager.Register(new NewCommand());
            commandManager.Register(new ShowCommand());
            commandManager.Register(new OpenCommand());
            commandManager.Register(new ImportCommand());

            while (true) //CLI Loop
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (input == "help")
                    commandManager.ShowHelp();
                else
                    commandManager.Execute(input);

                Console.WriteLine("\nDrücke ENTER um fortzufahren...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
