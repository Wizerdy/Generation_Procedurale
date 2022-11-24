using UnityEngine;

public abstract class InflictDamage : MonoBehaviour
{
    public float damageDelay;
    public uint damageOnTouch;

    private Life lastTouchedLife;
    private Transform lastTouchedTransform;

    private float lastInflictedDamageTime;
    private BulletController bulletController;
    private WeaponType _type;
    private void Start() {
        bulletController = GetComponent<BulletController>();
        if (bulletController) {
            _type = bulletController.type;
        }
    }

    private void TryToDamage(Transform touchedTransform)
    {
        if (Time.time < lastInflictedDamageTime + damageDelay)
            return;
        lastTouchedLife.OnCollision(touchedTransform, _type);
        lastTouchedLife.TakeDamage(damageOnTouch);
        lastInflictedDamageTime = Time.time;
    }

    protected void CheckObjectToDamage(Transform touchedTransform) {
        if (touchedTransform != lastTouchedTransform) {
            lastTouchedTransform = touchedTransform;
            lastTouchedLife = lastTouchedTransform.GetComponent<Life>();
        }

        if (lastTouchedLife != null)
            TryToDamage(touchedTransform);
    }
}
