using Lumify.Interfaces;
using Lumify.Models;
using Lumify.Utilities;

namespace Lumify.ListCommands
{
    public class RemoveCommand : IListCommand
    {
        public string Name => "remove";
        public string Description => "Entfernt ein Material aus der Liste: remove <material>";

        public void Execute(string[] args, MaterialList materialList, string basePath)
        {
            if(args.Length < 2)
            {
                Console.WriteLine($"| {Emojis.Cross} | Nutzung: remove <material>");
                return;
            }

            string material = args[1];

            if (materialList.Items.ContainsKey(material))
            {
                materialList.Items.Remove(material);
                Console.WriteLine($"| {Emojis.Check} |{material} entfernt.");
            }
            else
                Console.WriteLine($"| {Emojis.Check} | Material {material} befindet sich nicht in der Liste");

        }
    }
}
