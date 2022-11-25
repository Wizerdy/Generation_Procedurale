using UnityEngine;

public class BulletController : MonoBehaviour
{
    public WeaponType type;
    private Rigidbody2DMovement movement;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite bullet;
    [SerializeField] private Sprite thunder;
    [SerializeField] private Sprite iron;
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
                spriteRenderer.sprite = thunder;
                break;
            case WeaponType.IronBall:
                spriteRenderer.sprite = iron;

                break;
            case WeaponType.Arrow:
                spriteRenderer.sprite = bullet;

                break;
            default:
                break;
        }
    }
}
