using Lumify.src;
using Lumify.src.Interfaces;
using Lumify.src.Utilities;

namespace Lumify.src.MainCommands
{
    public class ImportCommand : IMainCommand
    {
        private readonly ImportHandler _importHandler;

        public ImportCommand(ImportHandler importHandler)
        {
            _importHandler = importHandler;
        }

        public string Name => "import";
        public string Description => "Importiert eine .litematic Datei in eine neue Liste";

        public void Execute(string[] args)
        {
            Console.WriteLine($"| {Emojis.Check} | Import-Modul gestartet.");
            Console.ResetColor();

            _importHandler.Run();
        }
    }
}
