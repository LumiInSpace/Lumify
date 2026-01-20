using Lumify.src.StatusHandling;
using Lumify.src.Utilities;

public class TaskStatusManager
{
    private readonly Dictionary<string, TaskLine> _tasks = new();

    public TaskLine Start(string key, string description)
    {
        lock (Console.Out) //damit ein weiterer Thread den Code nicht erreicht bevor der erste fertig ist
        {
            int lineIndex = Console.CursorTop;
            Console.WriteLine($"[   ] {description}");
            var task = new TaskLine(key, description, lineIndex);
            _tasks[key] = task;
            return task;
        }
    }

    public void Success(string key)
    {
        if (_tasks.TryGetValue(key, out var task))
            task.UpdateStatus($"{Emojis.Check}", ConsoleColor.Green);
    }

    public void Fail(string key)
    {
        if (_tasks.TryGetValue(key, out var task))
            task.UpdateStatus($"{Emojis.Cross}", ConsoleColor.Red);
    }
}