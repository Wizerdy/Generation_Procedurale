using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum WeaponType {
    None,
    ThunderBall,
    IronBall,
    Arrow
}

public class PlayerWeapons : MonoBehaviour
{
    public WeaponType leftWeapon;
    public WeaponType rightWeapon;
    [SerializeField] private Weapon thunderBall;
    [SerializeField] private Weapon ironBall;
    [SerializeField] private Weapon arrow;

    public UnityEvent<WeaponType> onWeaponPickUp;

    public bool PickUpWeapon(WeaponType type) {
        if (leftWeapon == WeaponType.None) {
            leftWeapon = type;
            onWeaponPickUp.Invoke(type);
            return true;
        }

        if (rightWeapon == WeaponType.None) {
            rightWeapon = type;
            return true;
        }

        return false;
    }

    public void DropLeftWeapon() {
        if (leftWeapon != WeaponType.None) {
            SpawnWeapon(leftWeapon);

            leftWeapon = WeaponType.None;
        }
    }
    public void DropRightWeapon() {
        if (rightWeapon != WeaponType.None) {
            SpawnWeapon(rightWeapon);
            rightWeapon = WeaponType.None;
        }
    }

    private void SpawnWeapon(WeaponType type) {
        switch (type) {
            case WeaponType.ThunderBall:
                var thunder = Instantiate(thunderBall, transform.position,transform.rotation);
                StartCoroutine(thunder.DropCoolDown());
                break;
            case WeaponType.IronBall:
                var iron = Instantiate(ironBall, transform.position, transform.rotation);
                StartCoroutine(iron.DropCoolDown());

                break;
            case WeaponType.Arrow:
                var ar = Instantiate(arrow, transform.position, transform.rotation);
                StartCoroutine(ar.DropCoolDown());

                break;
            default:
                break;
        }
    }
}
