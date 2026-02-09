using Lumify.src.Application.Contracts;
using Lumify.src.Interfaces;
using Lumify.src.Models;
using Lumify.src.Utilities;

namespace Lumify.src.ListCommands
{
    public class RemoveCommand : IListCommand
    {
        private readonly IMaterialService _materialService;

        public RemoveCommand(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        public string Name => "remove";
        public string Description => "Entfernt ein Material aus der Liste: remove <material>";

        public void Execute(string[] args, MaterialList materialList, string basePath)
        {
            if(args.Length < 2)
            {
                Console.WriteLine($"| {Emojis.Cross} | Nutzung: remove <material>");
                return;
            }

            string material = args[1].ToLowerInvariant();
            if (_materialService.Remove(materialList, material))
            {
                Console.WriteLine($"| {Emojis.Check} |{material} entfernt.");
            }
            else
            {
                Console.WriteLine($"| {Emojis.Warning} | Material {material} befindet sich nicht in der Liste");
            }
        }
    }
}
