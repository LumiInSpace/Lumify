using Lumify.src.Application.Contracts;
using Lumify.src.Interfaces;
using Lumify.src.Models;
using Lumify.src.Utilities;

namespace Lumify.src.ListCommands;

public class AddCommand : IListCommand
{
    private readonly IMaterialService _materialService;

    public AddCommand(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    public string Name => "add";
    public string Description => "Fügt ein Material mit Menge hinzu: add <name> <anzahl>";
    
    public void Execute(string[] args, MaterialList list, string basePath)
    {
        if (args.Length < 3 || !int.TryParse(args[2], out int amount))
        {
            Console.WriteLine($"| {Emojis.Cross} | Nutzung: add <material> <anzahl>");
            return;
        }
        
        string material = args[1];
        _materialService.Add(list, material, amount);
        string materialKey = material.ToLowerInvariant();
        
        Console.WriteLine($"| {Emojis.Check} |{amount}x {materialKey} hinzugefügt. Gesamt: {list.Items[materialKey]}");
    }
}
