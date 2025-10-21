using Lumify.Models;

namespace Lumify.Interfaces
{
    public interface IImportCommand
    {
        public string FileExtension { get; }
        public Dictionary<string, int> Execute(string filePath);
    }
}
