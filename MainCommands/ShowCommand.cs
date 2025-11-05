using Lumify.Interfaces;

namespace Lumify.MainCommands
{
    public class ShowCommand : IMainCommand
    {
        public string Name => "show";
        public string Description => "Listet alle gespeicherten Projekte auf";

        public void Execute(string basePath, string[] args)
        {
            var files = Directory.GetFiles(basePath, "*.json");

            if (files.Length == 0)
            {
                Console.WriteLine("Keine Projekte gefunden.");
                return;
            }

            Console.WriteLine("📜 Projekte:");
            foreach (var f in files)
                Console.WriteLine(" - " + Path.GetFileNameWithoutExtension(f));
        }
    }
}