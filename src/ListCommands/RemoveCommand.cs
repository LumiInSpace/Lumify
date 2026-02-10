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
        public string Description => "Remove a material from the list: remove <material>";

        public void Execute(string[] args, MaterialList materialList, string basePath)
        {
            if(args.Length < 2)
            {
                Console.WriteLine($"| {Emojis.Cross} | Usage: remove <material>");
                return;
            }

            string material = args[1].ToLowerInvariant();
            if (_materialService.Remove(materialList, material))
            {
                Console.WriteLine($"| {Emojis.Check} | {material} removed.");
            }
            else
            {
                Console.WriteLine($"| {Emojis.Warning} | Material {material} is not in the list");
            }
        }
    }
}
