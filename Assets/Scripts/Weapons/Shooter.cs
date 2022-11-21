using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{
    
    public Transform bullet;
    public float fireDelay;
    public Animator anim;
    public AnimationClip shootingAnim;
    public ParticleSystem psSmoke;

    private bool isShooting;
    private Vector2 lastDirection;
    private Transform bulletsParent;
    private float lastShootTime;
    private Coroutine cor;
    
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
        if (cor == null) {
            cor = StartCoroutine(Shooting(shootingAnim.length / 6));
        }
        //anim.SetTrigger("Shoot");
        //var bulletPosition = transform.position;
        //var newBullet = Instantiate(bullet, bulletPosition, Quaternion.identity, bulletsParent);
        //newBullet.up = transform.up;
        //lastShootTime = Time.time;
    }

    private IEnumerator Shooting(float time) {
        anim.SetTrigger("Shoot");
        psSmoke.Play();
        yield return new WaitForSeconds(time);
        var bulletPosition = transform.position;
        var newBullet = Instantiate(bullet, bulletPosition, Quaternion.identity, bulletsParent);
        newBullet.up = transform.up;
        lastShootTime = Time.time;
        cor = null;
    }
}
