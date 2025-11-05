using Lumify.Interfaces;
using Lumify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumify
{
    public class MainCommandManager
    {
        private readonly Dictionary<string, IMainCommand> _commands = new();

        public void Register(IMainCommand command)
        {
            _commands[command.Name] = command;
        }

        public bool Execute(string input, string basePath)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string cmdName = parts[0].ToLower();
            var args = parts.Skip(1).ToArray();

            if (_commands.TryGetValue(cmdName, out var cmd))
            {
                cmd.Execute(basePath, args);
                return true;
            }

            Console.WriteLine("| ❌ | Unbekannter Befehl. Tippe 'help' für eine Liste.");
            return false;
        }

        public void ShowHelp()
        {
            Console.WriteLine("📜 Verfügbare Befehle:");
            Console.WriteLine("- help: Zeigt diese Liste an.");
            foreach (var c in _commands.Values)
                Console.WriteLine($"- {c.Name}: {c.Description}");
        }
    }
}
