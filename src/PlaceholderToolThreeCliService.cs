using Lumify.src.Interfaces;

namespace Lumify.src;

public class PlaceholderToolThreeCliService : IToolCliService
{
    public int MenuNumber => 3;
    public string MenuLabel => "Placeholder";

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Placeholder Tool [3]");
            Console.WriteLine("This module is not implemented yet.");
            Console.WriteLine("Type 'start' to return to the start screen.");
            Console.Write("\nplaceholder3> ");

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
