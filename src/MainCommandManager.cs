using Lumify.src.Interfaces;
using Lumify.src.Utilities;

namespace Lumify.src
{
    public class MainCommandManager
    {
        private readonly Dictionary<string, IMainCommand> _commands = new();

        public void Register(IMainCommand command)
        {
            _commands[command.Name] = command;
        }

        public bool Execute(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string cmdName = parts[0].ToLower();
            var args = parts.Skip(1).ToArray();

            if (_commands.TryGetValue(cmdName, out var cmd))
            {
                cmd.Execute(args);
                return true;
            }

            Console.WriteLine($"| {Emojis.Cross} | Unknown command. Type 'help' for a list.");
            return false;
        }

        public void ShowHelp()
        {
            Console.WriteLine($"{Emojis.List} Commands");
            Console.WriteLine(new string('─', 48));
            Console.WriteLine("help   Show this list");

            foreach (var command in _commands.Values.OrderBy(c => c.Name))
            {
                Console.WriteLine($"{command.Name.PadRight(6)} {command.Description}");
            }

            Console.WriteLine(new string('─', 48));
        }

        public IReadOnlyList<IMainCommand> GetCommands()
        {
            return _commands.Values.OrderBy(c => c.Name).ToList();
        }
    }
}
