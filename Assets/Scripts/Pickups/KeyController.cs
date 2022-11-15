using UnityEngine;

public class KeyController : Pickup
{
    [SerializeField, Range(0,1)]
    private float lerpFactor;
    private bool shouldFollow;
    private Collider2D myCollider2D;

    private void Awake()
    {
        myCollider2D = GetComponent<Collider2D>();
    }

    protected override void PickUp(Inventory colliderInventory)
    {
        base.PickUp(colliderInventory);
        shouldFollow = true;
        myCollider2D.enabled = false;
    }

    private void Update()
    {
        if(!shouldFollow || ownerInventory == null)
            return;

        var offset = myCollider2D.offset;
        var endPosition = (Vector2)ownerInventory.transform.position - offset;
        transform.position = Vector2.Lerp(transform.position, endPosition, lerpFactor);
    }
}
