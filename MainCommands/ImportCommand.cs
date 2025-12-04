using Lumify.Interfaces;
using Lumify.Utilities;

namespace Lumify.MainCommands
{
    public class ImportCommand : IMainCommand
    {
        public string Name => "import";
        public string Description => "Importiert eine .litematic Datei in eine neue Liste";

        public void Execute(string basePath, string[] args)
        {
            Console.WriteLine($"| {Emojis.Check} | Import-Modul gestartet.");
            Console.ResetColor();

            ImportHandler.Run();
        }
    }
}