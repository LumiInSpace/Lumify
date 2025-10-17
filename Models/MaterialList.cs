namespace Lumify.Models
{
    public class MaterialList
    {
        public MaterialList(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public Dictionary<string, int> Items { get; set; } = new();
    }
}
