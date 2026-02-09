using Lumify.src.Models;

namespace Lumify.src.Application.Contracts;

public interface IMaterialListRepository
{
    bool Exists(string filePath);
    MaterialList? Load(string filePath);
    void Save(string filePath, MaterialList list);
    string[] GetFiles(string directoryPath, string searchPattern);
}
