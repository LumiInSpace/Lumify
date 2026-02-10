using Lumify.src.Interfaces;

namespace Lumify.src;

public class PlaceholderToolTwoCliService : IToolCliService
{
    public int MenuNumber => 2;
    public string MenuLabel => "Placeholder";

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Placeholder Tool [2]");
            Console.WriteLine("This module is not implemented yet.");
            Console.WriteLine("Type 'start' to return to the start screen.");
            Console.Write("\nplaceholder2> ");

            string? input = Console.ReadLine()?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            if (input == "start" || input == "back")
            {
                return;
            }
        }
    }
}
