using Lumify.MainCommands;
using System.Collections.Generic;
using System.Text;

namespace Lumify
{
    public class MainHandler
    {
        private static readonly string MaterialListsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lumify", "Lists");
        //Windows: C:\Users\<User>\AppData\Roaming\Lumify\Lists
        //Linux: /home/<user>/.config/Lumify/Lists
        //MacOS: /Users/<user>/Library/Application Support/Lumify/Lists

        public void Initialize()
        {
            Console.Title = "Lumify";

            Console.OutputEncoding = Encoding.UTF8;

            Console.ForegroundColor = ConsoleColor.Green;
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("| ✅ | UTF-8 Codierung verfügbar. ^_~");
                Console.ResetColor();
            }

            Console.WriteLine("\n");

            if (!Directory.Exists(MaterialListsPath))
            {

                Directory.CreateDirectory(MaterialListsPath);
            }
        }

        public void Run()
        {
            var commandManager = new MainCommandManager();

            commandManager.Register(new NewCommand());
            commandManager.Register(new ShowCommand());
            commandManager.Register(new OpenCommand());
            commandManager.Register(new ImportCommand());

            while (true)
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (input == "help")
                    commandManager.ShowHelp();
                else
                    commandManager.Execute(input, MaterialListsPath);

                Console.WriteLine("\nDrücke ENTER um fortzufahren...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
