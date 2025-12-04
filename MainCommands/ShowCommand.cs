using Lumify.Interfaces;
using Lumify.Utilities;

namespace Lumify.MainCommands
{
    public class ShowCommand : IMainCommand
    {
        public string Name => "show";
        public string Description => "Listet alle gespeicherten Projekte auf";

        public void Execute(string[] args)
        {
            var files = Directory.GetFiles(GlobalVariables.MaterialListPath, "*.lumify");

            if (files.Length == 0)
            {
                Console.WriteLine("Keine Projekte gefunden.");
                return;
            }

            Console.WriteLine($"{Emojis.List} Projekte:");
            foreach (var f in files)
                Console.WriteLine(" - " + Path.GetFileNameWithoutExtension(f));
        }
    }
}