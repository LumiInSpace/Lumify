using Lumify.src.Application.Contracts;
using Lumify.src.Models;

namespace Lumify.src.Application.Services;

public class MaterialService : IMaterialService
{
    public void Add(MaterialList list, string material, int amount)
    {
        string key = material.ToLowerInvariant();
        if (list.Items.ContainsKey(key))
        {
            list.Items[key] += amount;
        }
        else
        {
            list.Items[key] = amount;
        }
    }

    public bool Remove(MaterialList list, string material)
    {
        string key = material.ToLowerInvariant();
        return list.Items.Remove(key);
    }
}
