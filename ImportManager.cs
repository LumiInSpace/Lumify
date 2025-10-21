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

        public Dictionary<string, int>? Execute(string filePath)
        {
            if(String.IsNullOrWhiteSpace(filePath)) return null;
            string[] parts = filePath.Split('.', StringSplitOptions.RemoveEmptyEntries);
            string fileExtension = parts.Last();

            if (_imports.TryGetValue(fileExtension, out var importCommand))
            {
                return importCommand.Execute(filePath);
            }
            else
            {
                Console.WriteLine("| ❌ | Dieser Dateintyp wird nicht unterstützt");
                return null;
            }
        }
    }
}
