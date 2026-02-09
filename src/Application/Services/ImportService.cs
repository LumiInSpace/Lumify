using Lumify.src.Application.Contracts;
using Lumify.src.Interfaces;

namespace Lumify.src.Application.Services;

public class ImportService : IImportService
{
    private readonly Dictionary<string, IImportCommand> _imports;

    public ImportService(IEnumerable<IImportCommand> imports)
    {
        _imports = imports.ToDictionary(i => i.FileExtension.ToLowerInvariant(), i => i);
    }

    public Dictionary<string, int>? ImportFromFile(string filePath, out string message)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            message = "Dateipfad fehlt.";
            return null;
        }

        filePath = filePath.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

        if (!File.Exists(filePath))
        {
            message = "Datei nicht gefunden";
            return null;
        }

        string extension = Path.GetExtension(filePath).TrimStart('.').ToLowerInvariant();
        if (!_imports.TryGetValue(extension, out var importCommand))
        {
            message = "Dieser Dateityp wird nicht unterstützt";
            return null;
        }

        var result = importCommand.Execute(filePath);
        if (result == null)
        {
            message = "Import fehlgeschlagen.";
            return null;
        }

        message = "Import erfolgreich.";
        return result;
    }
}
