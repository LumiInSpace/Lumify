using System.Windows.Input;
using Lumify.Models;
using Lumify.Interfaces;

namespace Lumify.Commands;

public class AddCommand : IUserCommand
{
    public string Name => "add";
    public string Description => "Fügt ein Material mit Menge hinzu: add <name> <anzahl>";
    
    public void Execute(string[] args, MaterialList list, string basePath)
    {
        if (args.Length < 3 || !int.TryParse(args[2], out int amount))
        {
            Console.WriteLine("| ❌ | Nutzung: add <material> <anzahl>");
            return;
        }
        
        string material = args[1].ToLower();
        if (list.Items.ContainsKey(material))
            list.Items[material] += amount;
        else
            list.Items[material] = amount;
        
        Console.WriteLine($"| ✅ |{amount}x {material} hinzugefügt. Gesamt: {list.Items[material]}");
    }
}