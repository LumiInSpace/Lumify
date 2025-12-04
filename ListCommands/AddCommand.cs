using Lumify.Models;
using Lumify.Interfaces;
using Lumify.Utilities;

namespace Lumify.ListCommands;

public class AddCommand : IListCommand
{
    public string Name => "add";
    public string Description => "Fügt ein Material mit Menge hinzu: add <name> <anzahl>";
    
    public void Execute(string[] args, MaterialList list, string basePath)
    {
        if (args.Length < 3 || !int.TryParse(args[2], out int amount))
        {
            Console.WriteLine($"| {Emojis.Cross} | Nutzung: add <material> <anzahl>");
            return;
        }
        
        string material = args[1].ToLower();
        if (list.Items.ContainsKey(material))
            list.Items[material] += amount;
        else
            list.Items[material] = amount;
        
        Console.WriteLine($"| {Emojis.Check} |{amount}x {material} hinzugefügt. Gesamt: {list.Items[material]}");
    }
}