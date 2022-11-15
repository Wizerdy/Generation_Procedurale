using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public PickupType type => pickupType;
    [SerializeField] private PickupType pickupType;
    protected Inventory ownerInventory;

    protected virtual void PickUp(Inventory colliderInventory)
    {
        ownerInventory = colliderInventory;
        ownerInventory.AddPickup(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Inventory colliderInventory))
        {
            PickUp(colliderInventory);
        }
    }
}
