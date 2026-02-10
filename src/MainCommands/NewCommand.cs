using Lumify.src.Application.Contracts;
using Lumify.src.Interfaces;
using Lumify.src.Utilities;

namespace Lumify.src.MainCommands
{
    public class NewCommand : IMainCommand
    {
        private readonly IProjectService _projectService;

        public NewCommand(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public string Name => "new";
        public string Description => "Create a new material list: new <name>";

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"| {Emojis.Warning} | Please provide a name. Usage: new <name>");
                return;
            }

            var name = args[0];
            bool created = _projectService.TryCreate(name, null, out string message, out _);
            Console.WriteLine(created
                ? $"| {Emojis.Check} | {message}"
                : $"| {Emojis.Warning} | {message}");
        }
    }
}
