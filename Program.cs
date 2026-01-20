using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lumify
{
    class Program
    {

        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<MainHandler>();
                })
                .Build();

            var main = host.Services.GetRequiredService<MainHandler>();
            main.Initialize();
            main.Run();
        }
    }
}