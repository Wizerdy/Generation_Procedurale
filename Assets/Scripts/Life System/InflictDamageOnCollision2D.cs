using UnityEngine;

public class InflictDamageOnCollision2D : InflictDamage 
{

    private void OnCollisionStay2D(Collision2D col)
    {
        CheckObjectToDamage(col.transform);
    }
}
