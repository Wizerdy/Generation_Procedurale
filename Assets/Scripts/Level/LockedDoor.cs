using System.Collections;
using System.Linq;
using UnityEngine;

public class LockedDoor : DoorController
{
    public float disappearTime;
    public GameObject openDoor;
    
    private LockedDoor connectedLockedDoor;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.TryGetComponent<Inventory>(out var inventory))
            return;
        TryOpen(inventory);
    }

    private void TryOpen(Inventory inventory)
    {
        var lastKey = inventory.allPickups.FirstOrDefault(pickup => pickup.type == PickupType.Key);
        if (lastKey == null)
            return;
        inventory.RemovePickup(lastKey);
        Open(lastKey);
    }

    private void Open(Pickup usedKey)
    {
        Destroy(usedKey.gameObject);
        Disappear();

        if (connectedLockedDoor == null)
            return;

        connectedLockedDoor.Disappear();
    }

    private void Disappear()
    {
        var startTime = Time.time;
        var myTransform = transform;
        Instantiate(openDoor, myTransform.position, myTransform.rotation, myTransform.parent);

        var spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(DisappearCoroutine());

        IEnumerator DisappearCoroutine()
        {
            while (spriteRenderer.color.a > 0)
            {
                var elapsedTime = Time.time - startTime;
                var completedPercent = elapsedTime / disappearTime;
                var newColor = spriteRenderer.color;
                newColor.a = 1 - completedPercent;
                spriteRenderer.color = newColor;
                yield return null;
            }

            Destroy(gameObject);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        connectedLockedDoor = connectedDoor as LockedDoor;
    }
}
