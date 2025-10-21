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
            using var fileStream = File.OpenRead(filePath);
            using var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
            var nbtFile = new NbtFile();
            nbtFile.LoadFromStream(gzipStream, NbtCompression.None);

            var root = nbtFile.RootTag;
            var regions = root.Get<NbtCompound>("Regions");
            var items = new Dictionary<string, int>();

            foreach (var regionEntry in regions)
            {
                var region = (NbtCompound)regionEntry;
                var palette = region.Get<NbtList>("BlockStatePalette");
                var blockStates = region.Get<NbtLongArray>("BlockStates").Value;
                var size = region.Get<NbtIntArray>("Size").Value;

                int totalBlocks = size[0] * size[1] * size[2];
                int bitsPerBlock = Math.Max(4, (int)Math.Ceiling(Math.Log2(palette.Count)));
                ulong mask = (1UL << bitsPerBlock) - 1;
                ulong[] data = Array.ConvertAll(blockStates, v => (ulong)v);

                int bitIndex = 0;
                for (int i = 0; i < totalBlocks; i++)
                {
                    int longIndex = bitIndex / 64;
                    int bitOffset = bitIndex % 64;

                    ulong blockData = data[longIndex] >> bitOffset;
                    int remainingBits = 64 - bitOffset;
                    if (remainingBits < bitsPerBlock)
                        blockData |= data[longIndex + 1] << remainingBits;

                    int paletteIndex = (int)(blockData & mask);
                    var blockTag = (NbtCompound)palette[paletteIndex];
                    string blockName = blockTag.Get<NbtString>("Name").Value;

                    // Überspringe Luft
                    if (blockName == "minecraft:air")
                    {
                        bitIndex += bitsPerBlock;
                        continue;
                    }

                    if (items.ContainsKey(blockName))
                        items[blockName]++;
                    else
                        items[blockName] = 1;

                    bitIndex += bitsPerBlock;
                }

            }

            return items;
        }
    }
}
