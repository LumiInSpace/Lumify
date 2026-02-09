namespace Lumify.src.Application.Contracts;

public interface IImportService
{
    Dictionary<string, int>? ImportFromFile(string filePath, out string message);
}
