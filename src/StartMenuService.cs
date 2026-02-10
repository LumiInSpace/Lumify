using Lumify.src.Configuration;
using Lumify.src.Interfaces;
using Lumify.src.Utilities;
using Microsoft.Extensions.Options;
using System.Text;

namespace Lumify.src
{
    public class StartMenuService
    {
        private readonly IReadOnlyDictionary<int, IToolCliService> _toolServices;
        private readonly LumifyOptions _options;

        public StartMenuService(IEnumerable<IToolCliService> toolServices, IOptions<LumifyOptions> options)
        {
            _options = options.Value;
            _toolServices = toolServices.ToDictionary(service => service.MenuNumber);
        }

        public void Initialize()
        {
            Console.Title = $"{_options.AppName} {_options.Version}";
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

            if (!Directory.Exists(_options.MaterialListPath))
            {
                Directory.CreateDirectory(_options.MaterialListPath);
            }
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                PrintWelcome();

                Console.Write("\nstart> ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if (input == "0" || input == "exit")
                {
                    break;
                }

                if (!int.TryParse(input, out int selection))
                {
                    PrintInvalidSelection();
                    continue;
                }

                if (_toolServices.TryGetValue(selection, out var service))
                {
                    service.Run();
                    continue;
                }

                PrintInvalidSelection();
            }
        }

        private void PrintWelcome()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintCentered(" _     _   _ __  __ ___ _____ _   _ ");
            PrintCentered("| |   | | | |  \\/  |_ _|  ___| | | |");
            PrintCentered("| |   | | | | |\\/| || || |_  | |_| |");
            PrintCentered("| |___| |_| | |  | || ||  _|  \\   / ");
            PrintCentered("|_____|\\___/|_|  |_|___|_|     |_| ");
            Console.ResetColor();

            Console.WriteLine();
            foreach (var service in _toolServices.OrderBy(entry => entry.Key))
            {
                PrintCentered($"[{service.Key}] {service.Value.MenuLabel}");
            }
            Console.WriteLine();
            PrintCentered("[0] Beenden");
        }

        private static void PrintCentered(string line)
        {
            int width = Console.WindowWidth > 0 ? Console.WindowWidth : 80;
            int leftPadding = Math.Max(0, (width - line.Length) / 2);
            Console.WriteLine(new string(' ', leftPadding) + line);
        }

        private static void PrintInvalidSelection()
        {
            Console.WriteLine("Ungueltige Auswahl. Bitte waehle eine gueltige Zahl.");
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }
}
