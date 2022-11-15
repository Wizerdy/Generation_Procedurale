using UnityEngine;

public abstract class InflictDamage : MonoBehaviour
{
    public float damageDelay;
    public uint damageOnTouch;

    private Life lastTouchedLife;
    private Transform lastTouchedTransform;

    private float lastInflictedDamageTime;

    private void TryToDamage()
    {
        if (Time.time < lastInflictedDamageTime + damageDelay)
            return;
        lastTouchedLife.TakeDamage(damageOnTouch);
        lastInflictedDamageTime = Time.time;
    }

    protected void CheckObjectToDamage(Transform touchedTransform)
    {
        if (touchedTransform != lastTouchedTransform)
        {
            lastTouchedTransform = touchedTransform;
            lastTouchedLife = lastTouchedTransform.GetComponent<Life>();
        }

        if (lastTouchedLife != null)
            TryToDamage();
    }
}
