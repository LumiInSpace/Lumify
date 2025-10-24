using fNbt;
using Lumify.Interfaces;
using System.IO.Compression;

namespace Lumify.Imports
{
    public class LitematicaImportCommand : IImportCommand
    {
        public string FileExtension => "litematic";

        public Dictionary<string, int>? Execute(string filePath)
        {
            TaskStatusManager statusManager = new TaskStatusManager();
            statusManager.Start("import_task", "Importiere Datei");

            try
            {
                // --- Datei öffnen ---
                statusManager.Start("open_file", "Datei öffnen");
                using var fileStream = File.OpenRead(filePath);
                if (!fileStream.CanRead) 
                {
                    statusManager.Fail("open_file"); throw new Exception();
                }
                statusManager.Success("open_file");

                // --- Entpacken und NBT laden ---
                statusManager.Start("read_nbt", "Dateistruktur auslesen");
                using var gzip = new GZipStream(fileStream, CompressionMode.Decompress);
                var nbtFile = new NbtFile();
                nbtFile.LoadFromStream(gzip, NbtCompression.None);
                statusManager.Success("read_nbt");

                // --- Regionen auswerten ---
                statusManager.Start("parse_regions", "Analysiere Regionen und Blöcke");
                var root = nbtFile.RootTag;
                var regions = root.Get<NbtCompound>("Regions");
                var items = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                foreach (var regionEntry in regions)
                {
                    var region = (NbtCompound)regionEntry;
                    var palette = region.Get<NbtList>("BlockStatePalette");

                    foreach (var tag in palette)
                    {
                        var blockCompound = (NbtCompound)tag;
                        string blockName = blockCompound.Get<NbtString>("Name").Value;

                        if (blockName == "minecraft:air") 
                            continue; // ignore air, because it's not a placeable Block



                        if (!items.ContainsKey(blockName))
                            items[blockName] = 0;

                        items[blockName]++;
                    }
                }

                statusManager.Success("parse_regions");
                statusManager.Success("import_task");
                Console.ReadLine();
                return items;
            }
            catch
            {
                statusManager.Fail("import_task");
                return null;
            }
        }
    }
}
