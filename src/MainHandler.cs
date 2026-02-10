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
            Console.WriteLine($"| {Emojis.Check} | Lumify started.");
            Console.ResetColor();

            int currentCodePage = Console.OutputEncoding.CodePage;
            if (currentCodePage != 65001)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"| {Emojis.Warning} | UTF-8 encoding is not enabled. Some characters may be displayed incorrectly.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"| {Emojis.Check} | UTF-8 encoding available.");
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

                Console.WriteLine("\nPress ENTER to continue...");
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

            Console.WriteLine("Manage material lists directly in the terminal.");
            Console.WriteLine();
            Console.WriteLine($"{Emojis.List} Quick Start");
            Console.WriteLine(new string('─', 48));

            foreach (var command in _commandManager.GetCommands())
            {
                Console.WriteLine($"{command.Name.PadRight(6)} {command.Description}");
            }

            Console.WriteLine("help   Show all commands");
            Console.WriteLine(new string('─', 48));
        }
    }
}
