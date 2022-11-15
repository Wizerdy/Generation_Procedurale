using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2DMovement movement;

    private void Awake()
    {
        movement = GetComponent<Rigidbody2DMovement>();
    }

    private void Start()
    {
       movement.SetDirection(transform.up); 
    }
}
