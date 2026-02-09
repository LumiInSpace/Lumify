using Lumify.src.Models;

namespace Lumify.src.Interfaces
{
    public interface IImportCommand
    {
        public string FileExtension { get; }
        public Dictionary<string, int>? Execute(string filePath);
    }
}
