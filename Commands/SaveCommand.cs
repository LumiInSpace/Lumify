using System.Text.Json;
using Lumify.Interfaces;
using Lumify.Models;

namespace Lumify.Commands;

public class SaveCommand : IUserCommand
{
    public string Name => "save";
    public string Description => "Speichert den aktuellen Zustand der Liste";

    public void Execute(string[] args, MaterialList list, string basePath)
    {
        try
        {
            string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(basePath, json);

            Console.WriteLine($"| ✅ | Projekt '{list.Name}' wurde erfolgreich gespeichert!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"| ❌ | Fehler beim Speichern: {ex.Message}");
        }
    }
}