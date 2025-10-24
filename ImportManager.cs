using Lumify.Interfaces;

namespace Lumify
{
    public class ImportManager()
    {
        private readonly Dictionary<string, IImportCommand> _imports = new();

        public void Register(IImportCommand import)
        {
            _imports[import.FileExtension] = import;
        }

        public void Execute(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath)) return;
            filePath = filePath.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            Console.WriteLine($"[DEBUG] Path: {Path.GetFullPath(filePath)}");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("| ❌ | Datei nicht gefunden");
                return;
            }
            string[] parts = filePath.Split('.', StringSplitOptions.RemoveEmptyEntries);
            string fileExtension = parts.Last();

            if (_imports.TryGetValue(fileExtension, out var importCommand))
            {
                ConvertAndSave(importCommand.Execute(filePath));
            }
            else
            {
                Console.WriteLine("| ❌ | Dieser Dateintyp wird nicht unterstützt");
                return;
            }
        }

        private void ConvertAndSave(Dictionary<string, int>? itemList)
        {
            if (itemList == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("| ❌ | Items konnten nicht extrahiert werden oder die Liste ist leer");
                Console.ResetColor();
                return;
            }

            // --- Entferne minecraft: ---

            Console.WriteLine("Extrahierte Items:");

            foreach (var item in itemList)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }
    }
}
