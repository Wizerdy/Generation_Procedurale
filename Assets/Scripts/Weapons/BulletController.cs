using UnityEngine;

public class BulletController : MonoBehaviour
{
    public WeaponType type;
    private Rigidbody2DMovement movement;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        movement = GetComponent<Rigidbody2DMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        movement.SetDirection(transform.up);
    }

    public void BulletInit(WeaponType _type) {
        type = _type;
        switch (type) {
            case WeaponType.ThunderBall:
                spriteRenderer.color = Color.yellow;
                break;
            case WeaponType.IronBall:
                spriteRenderer.color = Color.gray;

                break;
            case WeaponType.Arrow:
                spriteRenderer.color = Color.red;

                break;
            default:
                break;
        }
    }
}
