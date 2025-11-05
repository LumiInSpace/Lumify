using Lumify.Models;
using System.Text;
using System.Text.Json;

namespace Lumify
{
    class Program
    {
        private const string MaterialListsPath = @"C:\ProgramData\Lumify\Lists";

        static void Main(string[] args)
        {
            MainHandler mainHandler = new MainHandler();
            mainHandler.Initialize();
            mainHandler.Run();
        }
    }
}