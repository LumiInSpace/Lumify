using Lumify.src.Application.Contracts;
using Lumify.src.Configuration;
using Lumify.src.Models;
using Microsoft.Extensions.Options;

namespace Lumify.src.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IMaterialListRepository _repository;
    private readonly LumifyOptions _options;

    public ProjectService(IMaterialListRepository repository, IOptions<LumifyOptions> options)
    {
        _repository = repository;
        _options = options.Value;
    }

    public bool TryCreate(string name, Dictionary<string, int>? items, out string message, out string filePath)
    {
        filePath = BuildProjectPath(name);

        if (string.IsNullOrWhiteSpace(name))
        {
            message = "Please provide a name.";
            return false;
        }

        if (_repository.Exists(filePath))
        {
            message = "Project already exists.";
            return false;
        }

        var list = new MaterialList(name, items ?? new Dictionary<string, int>());
        _repository.Save(filePath, list);
        message = $"Project '{name}' created!";
        return true;
    }

    public bool TryOpen(string name, out MaterialList list, out string filePath, out string message)
    {
        filePath = BuildProjectPath(name);
        list = new MaterialList(name);

        if (!_repository.Exists(filePath))
        {
            message = "Project not found.";
            return false;
        }

        var loaded = _repository.Load(filePath);
        list = loaded ?? new MaterialList(name);
        message = $"Project '{name}' opened.";
        return true;
    }

    public bool TrySave(MaterialList list, string filePath, out string message)
    {
        try
        {
            _repository.Save(filePath, list);
            message = $"Project '{list.Name}' was saved successfully!";
            return true;
        }
        catch (Exception ex)
        {
            message = $"Error while saving: {ex.Message}";
            return false;
        }
    }

    public IReadOnlyList<string> GetProjectNames()
    {
        string[] files = _repository.GetFiles(_options.MaterialListPath, "*.lumify");
        return files.Select(Path.GetFileNameWithoutExtension).Where(n => !string.IsNullOrWhiteSpace(n)).Cast<string>().ToList();
    }

    private string BuildProjectPath(string name)
    {
        return Path.Combine(_options.MaterialListPath, $"{name}.lumify");
    }
}
