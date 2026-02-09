using Lumify.src.Configuration;
using Lumify.src.Interfaces;
using Lumify.src.Utilities;
using Microsoft.Extensions.Options;
using System.Text;

namespace Lumify.src
{
    public class MainHandler
    {
        private readonly MainCommandManager _commandManager;
        private readonly LumifyOptions _options;

        public MainHandler(MainCommandManager commandManager, IEnumerable<IMainCommand> commands, IOptions<LumifyOptions> options)
        {
            _commandManager = commandManager;
            _options = options.Value;

            foreach (var command in commands)
            {
                _commandManager.Register(command);
            }
        }

        public void Initialize()
        {
            Console.Title = _options.AppName;
            Console.Clear();

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

            if (!Directory.Exists(_options.MaterialListPath))
            {
                Directory.CreateDirectory(_options.MaterialListPath);
            }

            PrintWelcome();
        }

        public void Run()
        {
            while (true) //CLI Loop
            {
                Console.Write("\nlumify> ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if (input == "help")
                    _commandManager.ShowHelp();
                else
                    _commandManager.Execute(input);

                Console.WriteLine("\nDrücke ENTER um fortzufahren...");
                Console.ReadLine();
                Console.Clear();
                PrintWelcome();
            }
        }

        private void PrintWelcome()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("LUMIFY");
            Console.ResetColor();

            Console.WriteLine("Verwalte Materiallisten direkt im Terminal.");
            Console.WriteLine();
            Console.WriteLine($"{Emojis.List} Schnellstart");
            Console.WriteLine(new string('─', 48));

            foreach (var command in _commandManager.GetCommands())
            {
                Console.WriteLine($"{command.Name.PadRight(6)} {command.Description}");
            }

            Console.WriteLine("help   Zeigt alle Befehle kompakt");
            Console.WriteLine(new string('─', 48));
        }
    }
}
