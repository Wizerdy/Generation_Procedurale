using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType type;
    [SerializeField] private float cooldDownTime = 2.0f;
    private Collider2D col;

    private void Awake() {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            if (col.GetComponent<PlayerWeapons>().PickUpWeapon(type)) {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator DropCoolDown() {
        col.enabled = false;
        yield return new WaitForSeconds(cooldDownTime);
        col.enabled = true;
    }
}
