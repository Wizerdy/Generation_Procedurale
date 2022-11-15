using UnityEngine;

public class Shooter : MonoBehaviour
{
    
    public Transform bullet;
    public float fireDelay;

    private bool isShooting;
    private Vector2 lastDirection;
    private Transform bulletsParent;
    private float lastShootTime;
    
    private void Awake()
    {
        bulletsParent = new GameObject($"{name} bullets").transform;
    }

    private void Start()
    {
        lastShootTime = fireDelay * -1;
    }

    public void StartShooting()
    {
        isShooting = true;
        TryToShoot();
    }
    
    private void Update()
    {
        if (isShooting)
            TryToShoot();
    }

    public void StopShooting()
    {
        isShooting = false;
    }
    
    private void TryToShoot()
    {
        if (Time.time < lastShootTime + fireDelay) return;
        var bulletPosition = transform.position;
        var newBullet = Instantiate(bullet, bulletPosition, Quaternion.identity, bulletsParent);
        newBullet.up = transform.up;
        lastShootTime = Time.time;
    }
}
