using System.Text.Json;
using Lumify.Interfaces;
using Lumify.Models;
using Lumify.Utilities;

namespace Lumify.ListCommands;

public class SaveCommand : IListCommand
{
    public string Name => "save";
    public string Description => "Speichert den aktuellen Zustand der Liste";

    public void Execute(string[] args, MaterialList list, string basePath)
    {
        try
        {
            string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(GlobalVariables.MaterialListPath, json);

            Console.WriteLine($"| {Emojis.Check} | Projekt '{list.Name}' wurde erfolgreich gespeichert!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"| {Emojis.Cross} | Fehler beim Speichern.");
        }
    }
}