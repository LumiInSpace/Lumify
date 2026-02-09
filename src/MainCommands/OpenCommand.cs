using Lumify.src;
using Lumify.src.Application.Contracts;
using Lumify.src.Interfaces;
using Lumify.src.Utilities;

namespace Lumify.src.MainCommands
{
    public class OpenCommand : IMainCommand
    {
        private readonly IProjectService _projectService;
        private readonly ListHandler _listHandler;

        public OpenCommand(IProjectService projectService, ListHandler listHandler)
        {
            _projectService = projectService;
            _listHandler = listHandler;
        }

        public string Name => "open";
        public string Description => "Öffnet ein bestehendes Projekt: open <name>";

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"| {Emojis.Warning} | Bitte Projektnamen angeben. Verwendung: open <name>");
                return;
            }

            string name = args[0];
            bool opened = _projectService.TryOpen(name, out var list, out string filePath, out string message);
            if (!opened)
            {
                Console.WriteLine($"| {Emojis.Cross} | {message}");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"| {Emojis.Check} | {message}");
            Console.ResetColor();

            _listHandler.Run(list, filePath);
        }
    }
}
