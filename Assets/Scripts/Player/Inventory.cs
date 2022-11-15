using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public IReadOnlyList<Pickup> allPickups => pickups.AsReadOnly();
    private readonly List<Pickup> pickups = new();

    public void AddPickup(IEnumerable<Pickup> toAdd)
    {
        pickups.AddRange(toAdd);
    }
    
    public void AddPickup(Pickup toAdd)
    {
        pickups.Add(toAdd);
    }

    public void RemovePickup(Pickup toRemove)
    {
        pickups.Remove(toRemove);
    }
}
