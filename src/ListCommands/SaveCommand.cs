using Lumify.src.Application.Contracts;
using Lumify.src.Interfaces;
using Lumify.src.Models;
using Lumify.src.Utilities;

namespace Lumify.src.ListCommands;

public class SaveCommand : IListCommand
{
    private readonly IProjectService _projectService;

    public SaveCommand(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public string Name => "save";
    public string Description => "Save the current state of the list";

    public void Execute(string[] args, MaterialList list, string basePath)
    {
        bool success = _projectService.TrySave(list, basePath, out string message);
        Console.WriteLine(success
            ? $"| {Emojis.Check} | {message}"
            : $"| {Emojis.Cross} | {message}");
    }
}
