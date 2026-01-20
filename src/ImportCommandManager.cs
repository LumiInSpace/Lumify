using Lumify.src.Interfaces;
using Lumify.src.Utilities;

namespace Lumify.src
{
    public class ImportCommandManager()
    {
        private readonly Dictionary<string, IImportCommand> _imports = new();

        public void Register(IImportCommand import)
        {
            _imports[import.FileExtension] = import;
        }

        public Dictionary<string, int>? Execute(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath)) return null;
            filePath = filePath.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"| {Emojis.Cross} | Datei nicht gefunden");
                return null;
            }
            string[] parts = filePath.Split('.', StringSplitOptions.RemoveEmptyEntries);
            string fileExtension = parts.Last();

            if (_imports.TryGetValue(fileExtension, out var importCommand))
            {
                return importCommand.Execute(filePath);
            }
            else
            {
                Console.WriteLine($"| {Emojis.Cross} | Dieser Dateintyp wird nicht unterstützt");
                return null;
            }
        }
    }
}