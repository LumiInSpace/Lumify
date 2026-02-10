using Lumify.src.Application.Contracts;
using Lumify.src.Interfaces;
using Lumify.src.Utilities;

namespace Lumify.src.MainCommands
{
    public class ShowCommand : IMainCommand
    {
        private readonly IProjectService _projectService;

        public ShowCommand(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public string Name => "show";
        public string Description => "List all saved projects";

        public void Execute(string[] args)
        {
            var names = _projectService.GetProjectNames();

            if (names.Count == 0)
            {
                Console.WriteLine("No projects found.");
                return;
            }

            Console.WriteLine($"{Emojis.List} Projects:");
            foreach (var name in names)
            {
                Console.WriteLine(" - " + name);
            }
        }
    }
}
