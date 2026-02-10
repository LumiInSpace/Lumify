using Lumify.src;
using Lumify.src.Application.Contracts;
using Lumify.src.Application.Services;
using Lumify.src.Configuration;
using Lumify.src.Imports;
using Lumify.src.Infrastructure.Persistence;
using Lumify.src.Interfaces;
using Lumify.src.ListCommands;
using Lumify.src.MainCommands;
using Lumify.src.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using MainShowCommand = Lumify.src.MainCommands.ShowCommand;

namespace Lumify
{
    class Program
    {

        static void Main(string[] args)
        {
            var informationalVersion =
                Assembly.GetEntryAssembly()?
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion
                ?? "dev";

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.Configure<LumifyOptions>(options =>
                    {
                        options.MaterialListPath = Path.Combine(options.BaseDirectory, "Lists");
                        options.Version = informationalVersion;
                    });

                    services.AddSingleton<IMaterialListRepository, FileMaterialListRepository>();
                    services.AddSingleton<IProjectService, ProjectService>();
                    services.AddSingleton<IMaterialService, MaterialService>();
                    services.AddSingleton<IImportService, ImportService>();

                    services.AddTransient<IImportCommand, LitematicaImportCommand>();

                    services.AddTransient<IMainCommand, NewCommand>();
                    services.AddTransient<IMainCommand, MainShowCommand>();
                    services.AddTransient<IMainCommand, OpenCommand>();
                    services.AddTransient<IMainCommand, ImportCommand>();

                    services.AddTransient<IListCommand, AddCommand>();
                    services.AddTransient<IListCommand, ShowListCommand>();
                    services.AddTransient<IListCommand, SaveCommand>();
                    services.AddTransient<IListCommand, RemoveCommand>();

                    services.AddSingleton<CliNavigationService>();
                    services.AddSingleton<MainCommandManager>();
                    services.AddTransient<ListCommandManager>();
                    services.AddTransient<ListCliService>();
                    services.AddTransient<ImportCliService>();

                    services.AddSingleton<IToolCliService, MaterialListsCliService>();
                    services.AddSingleton<IToolCliService, PlaceholderToolTwoCliService>();
                    services.AddSingleton<IToolCliService, PlaceholderToolThreeCliService>();
                    services.AddSingleton<StartMenuService>();
                })
                .Build();

            var startMenu = host.Services.GetRequiredService<StartMenuService>();
            startMenu.Initialize();
            startMenu.Run();
        }
    }
}
