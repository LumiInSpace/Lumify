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
                            break; // Sicherheitsabbruch – keine Daten mehr

                        long current = blockStates[longIndex];
                        long next = (longIndex + 1 < blockStates.Length) ? blockStates[longIndex + 1] : 0L;

                        // Bits extrahieren, ggf. über Long-Grenze hinweg
                        long value = (current >> startBit) | (next << (64 - startBit));
                        int paletteIndex = (int)(value & ((1L << bitsPerBlock) - 1));

                        if (paletteIndex < 0 || paletteIndex >= paletteList.Count)
                        {
                            // Ungültiger Index, überspringen
                            bitIndex += bitsPerBlock;
                            continue;
                        }

                        string blockName = paletteList[paletteIndex];
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
