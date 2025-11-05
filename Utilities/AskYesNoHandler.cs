namespace Lumify.Utilities
{
    public static class AskYesNoHandler
    {
        static public bool AskYesNo(string message)
        {
            Console.WriteLine($"{message} [j/n]");
            while (true)
            {
                var key = Console.ReadKey(intercept: true).KeyChar.ToString().ToLower();
                if (key == "j" || key == "y")
                {
                    return true;
                }
                if (key == "n")
                {
                    return false;
                }
            }
        }
    }
}
