using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    public bool isInvincible;
    public uint startLife;
    public UnityEvent<uint> onDamageTaken;

    public uint currentLife { get; private set; }
    public IEnumerator onDie;

    private bool isAlive = true;

    private void Start()
    {
        currentLife = startLife;
    }

    public void TakeDamage(uint damage)
    {
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
