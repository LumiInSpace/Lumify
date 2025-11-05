using Lumify.Models;

namespace Lumify.Interfaces;

public interface IListCommand
{
    string Name { get; }
    string Description { get; }
    void Execute(string[] args, MaterialList list, string basePath);
}