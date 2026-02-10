using Lumify.src;
using Lumify.src.Interfaces;
using Lumify.src.Utilities;

namespace Lumify.src.MainCommands
{
    public class ImportCommand : IMainCommand
    {
        private readonly ImportCliService _importService;

        public ImportCommand(ImportCliService importService)
        {
            _importService = importService;
        }

        public string Name => "import";
        public string Description => "Import a .litematic file into a new list";

        public void Execute(string[] args)
        {
            Console.WriteLine($"| {Emojis.Check} | Import module started.");
            Console.ResetColor();

            _importService.Run();
        }
    }
}
