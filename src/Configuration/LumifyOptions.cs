namespace Lumify.src.Configuration;

public class LumifyOptions
{
    public string AppName { get; } = "Lumify";
    public string Version { get; set; } = "";
    public string BaseDirectory { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Lumify");
    public string MaterialListPath { get; set; } = "";
}
