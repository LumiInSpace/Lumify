using Lumify.src.Interfaces;
using Lumify.src.Models;
using Lumify.src.Utilities;
using System;

namespace Lumify.src.ListCommands
{
    public class ShowListCommand : IListCommand
    {
        public string Name => "show";
        public string Description => "Show all materials.";

        public void Execute(string[] args, MaterialList list, string basePath)
        {
            Console.WriteLine();
            
            if (list.Items.Count == 0)
            {
                Console.WriteLine("No materials available.");
                return;
            }

            Console.WriteLine($"{Emojis.Package} Materials: \n");
            foreach (var item in list.Items)
                Console.WriteLine($"- {item.Key}: {item.Value}");
        }
    }
}
