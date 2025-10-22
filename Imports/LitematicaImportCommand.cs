using fNbt;
using Lumify.Interfaces;
using System.IO;
using System.IO.Compression;

namespace Lumify.Imports
{
    public class LitematicaImportCommand : IImportCommand
    {
        public string FileExtension => "litematic";

        public Dictionary<string, int> Execute(string filePath)
        {
            Console.WriteLine();
            Console.WriteLine("Open"
            using var fileStream = File.OpenRead(filePath);
            using var gzip = new GZipStream(fileStream, CompressionMode.Decompress);
            var nbtFile = new NbtFile();
            nbtFile.LoadFromStream(gzip, NbtCompression.None);

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

                    if (blockName == "minecraft:air") continue; // ignore air, because it's not a placeable Block



                    if (!items.ContainsKey(blockName))
                        items[blockName] = 0;

                    items[blockName]++;
                }
            }

            return items;
        }
    }
}
