using Lumify.Models;

namespace Lumify.Interfaces;

public interface IUserCommand
{
    string Name { get; }
    string Description { get; }
    void Execute(string[] args, MaterialList list, string basePath);
}