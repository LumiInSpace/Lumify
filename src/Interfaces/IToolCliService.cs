namespace Lumify.src.Interfaces;

public interface IToolCliService
{
    int MenuNumber { get; }
    string MenuLabel { get; }
    void Run();
}
