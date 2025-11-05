namespace Lumify.Models
{
    public class MaterialList
    {
        public MaterialList(string name, Dictionary<string, int> items = null!)
        {
            Name = name;
            Items = items ?? new Dictionary<string, int>();
        }

        public string Name { get; set; }
        public Dictionary<string, int> Items { get; set; } = new();
    }
}
