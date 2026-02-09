using Lumify.src;
using Lumify.src.Application.Contracts;
using Lumify.src.Application.Services;
using Lumify.src.Configuration;
using Lumify.src.Imports;
using Lumify.src.Infrastructure.Persistence;
using Lumify.src.Interfaces;
using Lumify.src.ListCommands;
using Lumify.src.MainCommands;
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
                    services.Configure<LumifyOptions>(options =>
                    {
                        options.MaterialListPath = Path.Combine(options.BaseDirectory, "Lists");
                    });

                    services.AddSingleton<IMaterialListRepository, FileMaterialListRepository>();
                    services.AddSingleton<IProjectService, ProjectService>();
                    services.AddSingleton<IMaterialService, MaterialService>();
                    services.AddSingleton<IImportService, ImportService>();

                    services.AddTransient<IImportCommand, LitematicaImportCommand>();

                    services.AddTransient<IMainCommand, NewCommand>();
                    services.AddTransient<IMainCommand, Lumify.src.MainCommands.ShowCommand>();
                    services.AddTransient<IMainCommand, OpenCommand>();
                    services.AddTransient<IMainCommand, ImportCommand>();

                    services.AddTransient<IListCommand, AddCommand>();
                    services.AddTransient<IListCommand, Lumify.src.ListCommands.ShowCommand>();
                    services.AddTransient<IListCommand, SaveCommand>();
                    services.AddTransient<IListCommand, RemoveCommand>();

                    services.AddSingleton<MainCommandManager>();
                    services.AddTransient<ListCommandManager>();
                    services.AddTransient<ListHandler>();
                    services.AddTransient<ImportHandler>();
                    services.AddSingleton<MainHandler>();
                })
                .Build();

            var main = host.Services.GetRequiredService<MainHandler>();
            main.Initialize();
            main.Run();
        }
    }
}
