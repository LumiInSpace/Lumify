using Lumify.src.Models;

namespace Lumify.src.Interfaces;

public interface IListCommand
{
    string Name { get; }
    string Description { get; }
    void Execute(string[] args, MaterialList list, string basePath);
}