using Lumify.Interfaces;
using Lumify.Models;
using System;

namespace Lumify.Core.Commands
{
    public class ShowCommand : IUserCommand
    {
        public string Name => "show";
        public string Description => "Zeigt alle Materialien an.";

        public void Execute(string[] args, MaterialList list, string basePath)
        {
            Console.WriteLine();
            
            if (list.Items.Count == 0)
            {
                Console.WriteLine("Keine Materialien vorhanden.");
                return;
            }

            Console.WriteLine("📦 Materialien: \n");
            foreach (var item in list.Items)
                Console.WriteLine($"- {item.Key}: {item.Value}");
        }
    }
}