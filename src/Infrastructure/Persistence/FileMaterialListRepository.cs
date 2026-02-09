using System.Text.Json;
using Lumify.src.Application.Contracts;
using Lumify.src.Models;

namespace Lumify.src.Infrastructure.Persistence;

public class FileMaterialListRepository : IMaterialListRepository
{
    public bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }

    public MaterialList? Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<MaterialList>(json);
    }

    public void Save(string filePath, MaterialList list)
    {
        string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public string[] GetFiles(string directoryPath, string searchPattern)
    {
        if (!Directory.Exists(directoryPath))
        {
            return Array.Empty<string>();
        }

        return Directory.GetFiles(directoryPath, searchPattern);
    }
}
