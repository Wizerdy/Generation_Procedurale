using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Life))]
public class InvincibleOnHit : MonoBehaviour
{
    public float invincibilityTime;

    private Life lifeComponent;

    private void Awake()
    {
        lifeComponent = GetComponent<Life>();
    }

    private void Start()
    {
        lifeComponent.onDamageTaken.AddListener(OnDamageTaken);
    }

    private void OnDamageTaken(uint lifePoint)
    {
        lifeComponent.isInvincible = true;
        StartCoroutine(Invincibility());

        IEnumerator Invincibility()
        {
            yield return new WaitForSeconds(invincibilityTime);
            lifeComponent.isInvincible = false;
        }
    }
}
