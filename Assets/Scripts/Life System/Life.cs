using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    public bool isInvincible;
    public uint startLife;
    public UnityEvent<uint> onDamageTaken;
    public UnityEvent<uint> onHit;
    public UnityEvent<Transform> onHitWithTransform;


    public uint currentLife { get; private set; }
    public IEnumerator onDie;

    private bool isAlive = true;

    private void Start()
    {
        currentLife = startLife;
    }

    public void OnCollision(Transform colTransform, WeaponType type) {
        if (colTransform.gameObject.tag == "Box") {
            if (type != WeaponType.IronBall) {
                colTransform.GetComponent<Life>().Heal(1);
            }
        }

        if (colTransform.gameObject.tag == "SimonLight") {
            if (type == WeaponType.ThunderBall) {
                colTransform.GetComponent<SimonLight>().LightTrigger();
            }
        }


        if (colTransform.gameObject.tag == "Ball") {
            if (type == WeaponType.Arrow) {
                colTransform.GetComponent<Ball>().OnHit();
            }
        }

    }
    public void Heal(uint ammount) {
        currentLife += ammount;
    }

    public void TakeDamage(uint damage)
    {
        Debug.Log("dead " + name);
        onHit.Invoke(damage);
        if (!isAlive || isInvincible) return;
        if (damage > currentLife)
            damage = currentLife;
        currentLife -= damage;
        onDamageTaken.Invoke(currentLife);
        if (currentLife == 0)
            Die();
    }

    private void Die()
    {
        StartCoroutine(DieCoroutine());

        IEnumerator DieCoroutine()
        {
            isAlive = false;
            if (onDie != null)
                yield return StartCoroutine(onDie);
            Destroy(gameObject);
        }
    }
}
