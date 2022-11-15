using UnityEngine;

public class InflictDamageOnTrigger2D : InflictDamage 
{

    private void OnTriggerStay2D(Collider2D col)
    {
        CheckObjectToDamage(col.transform);
    }
}
