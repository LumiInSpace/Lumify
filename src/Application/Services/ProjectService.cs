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
            message = "Bitte Namen angeben.";
            return false;
        }

        if (_repository.Exists(filePath))
        {
            message = "Projekt existiert bereits.";
            return false;
        }

        var list = new MaterialList(name, items ?? new Dictionary<string, int>());
        _repository.Save(filePath, list);
        message = $"Projekt '{name}' erstellt!";
        return true;
    }

    public bool TryOpen(string name, out MaterialList list, out string filePath, out string message)
    {
        filePath = BuildProjectPath(name);
        list = new MaterialList(name);

        if (!_repository.Exists(filePath))
        {
            message = "Projekt nicht gefunden.";
            return false;
        }

        var loaded = _repository.Load(filePath);
        list = loaded ?? new MaterialList(name);
        message = $"Projekt '{name}' geöffnet.";
        return true;
    }

    public bool TrySave(MaterialList list, string filePath, out string message)
    {
        try
        {
            _repository.Save(filePath, list);
            message = $"Projekt '{list.Name}' wurde erfolgreich gespeichert!";
            return true;
        }
        catch (Exception ex)
        {
            message = $"Fehler beim Speichern: {ex.Message}";
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
