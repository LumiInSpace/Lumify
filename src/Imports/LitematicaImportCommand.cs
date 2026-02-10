using fNbt;
using Lumify.src.Interfaces;
using System.IO.Compression;

namespace Lumify.src.Imports
{
    public class LitematicaImportCommand : IImportCommand
    {
        public string FileExtension => "litematic";

        public Dictionary<string, int>? Execute(string filePath)
        {
            TaskStatusManager statusManager = new TaskStatusManager();
            statusManager.Start("import_task", "Importing file");

            try
            {
                // --- Datei öffnen ---
                statusManager.Start("open_file", "Opening file");
                using var fileStream = File.OpenRead(filePath);
                if (!fileStream.CanRead) 
                {
                    statusManager.Fail("open_file"); throw new Exception();
                }
                statusManager.Success("open_file");

                // --- Entpacken und NBT laden ---
                statusManager.Start("read_nbt", "Reading file structure");
                using var gzip = new GZipStream(fileStream, CompressionMode.Decompress);
                var nbtFile = new NbtFile();
                nbtFile.LoadFromStream(gzip, NbtCompression.None);
                statusManager.Success("read_nbt");

                // --- Regionen auswerten ---
                statusManager.Start("parse_regions", "Analyzing regions and blocks");
                var root = nbtFile.RootTag;
                var regions = root.Get<NbtCompound>("Regions");
                var items = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                foreach (var regionEntry in regions)
                {
                    var region = (NbtCompound)regionEntry;
                    var palette = region.Get<NbtList>("BlockStatePalette");
                    var blockStates = region.Get<NbtLongArray>("BlockStates").Value;

                    var paletteList = new List<string>();
                    foreach (NbtCompound tag in palette)
                        paletteList.Add(tag.Get<NbtString>("Name").Value);

                    int bitsPerBlock = Math.Max(2, (int)Math.Ceiling(Math.Log2(paletteList.Count)));
                    int blocksPerLong = 64 / bitsPerBlock;
                    int totalBlocks = (blockStates.Length * 64) / bitsPerBlock;

                    int bitIndex = 0;
                    for (int i = 0; i < totalBlocks; i++)
                    {
                        int longIndex = bitIndex / 64;
                        int startBit = bitIndex % 64;

                        if (longIndex >= blockStates.Length)
                            break;

                        long current = blockStates[longIndex];
                        long next = (longIndex + 1 < blockStates.Length) ? blockStates[longIndex + 1] : 0L;

                       
                        long value = (current >> startBit) | (next << (64 - startBit));
                        int paletteIndex = (int)(value & ((1L << bitsPerBlock) - 1));

                        if (paletteIndex < 0 || paletteIndex >= paletteList.Count)
                        {
                            bitIndex += bitsPerBlock;
                            continue;
                        }

                        string? blockName = NormalizeBlockName(paletteList[paletteIndex]);
                        if (blockName == null || blockName == "minecraft:air")
                        {
                            bitIndex += bitsPerBlock;
                            continue;
                        }
                        if (blockName != "minecraft:air")
                        {
                            if (!items.ContainsKey(blockName))
                                items[blockName] = 0;
                            items[blockName]++;
                        }

                        bitIndex += bitsPerBlock;
                    }
                }

                statusManager.Success("parse_regions");
                statusManager.Success("import_task");
                return items;
            }
            catch
            {
                statusManager.Fail("import_task");
                return null;
            }
        }
        
        private static string? NormalizeBlockName(string name)
        {
            //replace all non-existing blocks with real blocks (I hope this fixes the issue)
            
            if (name.EndsWith("_head") || name.Contains("piston_head"))
                return null;

            if (name.EndsWith("_upper"))
                return null;
            if (name.EndsWith("_lower"))
                return name.Replace("_lower", "");

            if (name.EndsWith("_bed_head"))
                return null;
            if (name.EndsWith("_bed_foot"))
                return name.Replace("_foot", "");

            if (name.Contains("piston_head"))
                return null;

            return name;
        }

    }
}
