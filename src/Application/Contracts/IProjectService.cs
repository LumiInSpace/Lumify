using Lumify.src.Models;

namespace Lumify.src.Application.Contracts;

public interface IProjectService
{
    bool TryCreate(string name, Dictionary<string, int>? items, out string message, out string filePath);
    bool TryOpen(string name, out MaterialList list, out string filePath, out string message);
    bool TrySave(MaterialList list, string filePath, out string message);
    IReadOnlyList<string> GetProjectNames();
}
