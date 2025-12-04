using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumify.StatusHandling
{
    public class TaskLine
    {
        public int LineIndex { get; set; }
        public string Description { get; }
        public string Key { get; }

        public TaskLine(string key, string description, int lineIndex)
        {
            Key = key;
            Description = description;
            LineIndex = lineIndex;
        }

        public void UpdateStatus(string symbol, ConsoleColor? color = null)
        {
            lock (Console.Out)
            {
                int currentTop = Console.CursorTop;
                Console.SetCursorPosition(0, LineIndex);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, LineIndex);

                if (color.HasValue)
                {
                    Console.ForegroundColor = color.Value;
                }

                Console.Write($"[ {symbol} ] {Description}");
                Console.ResetColor();

                Console.SetCursorPosition(0, currentTop);
            }
        }
    }
}
