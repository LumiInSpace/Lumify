using Lumify.src.Models;

namespace Lumify.src.Application.Contracts;

public interface IMaterialService
{
    void Add(MaterialList list, string material, int amount);
    bool Remove(MaterialList list, string material);
}
